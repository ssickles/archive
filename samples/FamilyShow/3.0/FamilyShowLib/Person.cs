using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Xml.Serialization;

namespace Microsoft.FamilyShowLib
{
    /// <summary>
    /// Representation for a single serializable Person.
    /// INotifyPropertyChanged allows properties of the Person class to
    /// participate as source in data bindings.
    /// </summary>
    [Serializable]
    public class Person : INotifyPropertyChanged, IEquatable<Person>
    {
        #region Fields and Constants

        // The constants specific to this class
        private static class Const
        {
            public const string DefaultFirstName = "Unknown";
        }

        private string id;
        private string firstName;
        private string lastName;
        private string middleName;
        private string suffix;
        private string nickName;
        private string marriedName;
        private Gender gender;
        private DateTime? birthDate;
        private string birthPlace;
        private DateTime? deathDate;
        private string deathPlace;
        private bool isLiving;
        private PhotoCollection photos;
        private Story story;
        private RelationshipCollection relationships;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the unique identifier for each person.
        /// </summary>
        [XmlAttribute]
        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged("Id");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name that occurs first in a given name
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (firstName != value)
                {
                    firstName = value;
                    OnPropertyChanged("FirstName");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the part of a given name that indicates what family the person belongs to. 
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (lastName != value)
                {
                    lastName = value;
                    OnPropertyChanged("LastName");
                    OnPropertyChanged("Name");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the name that occurs between the first and last name.
        /// </summary>
        public string MiddleName
        {
            get { return middleName; }
            set
            {
                if (middleName != value)
                {
                    middleName = value;
                    OnPropertyChanged("MiddleName");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets the person's name in the format FirstName LastName.
        /// </summary>
        public string Name
        {
            get
            {
                string name = "";
                if (!string.IsNullOrEmpty(firstName))
                    name += firstName;
                if (!string.IsNullOrEmpty(lastName))
                    name += " " + lastName;
                return name;
            }
        }

        /// <summary>
        /// Gets the person's fully qualified name: Firstname MiddleName LastName Suffix
        /// </summary>
        public string FullName
        {
            get
            {
                string fullName = "";
                if (!string.IsNullOrEmpty(firstName))
                    fullName += firstName;
                if (!string.IsNullOrEmpty(middleName))
                    fullName += " " + middleName;
                if (!string.IsNullOrEmpty(lastName))
                    fullName += " " + lastName;
                if (!string.IsNullOrEmpty(suffix))
                    fullName += " " + suffix;
                return fullName;
            }
        }

        /// <summary>
        /// Gets or sets the text that appear behind the last name providing additional information about the person.
        /// </summary>
        public string Suffix
        {
            get { return suffix; }
            set
            {
                if (suffix != value)
                {
                    suffix = value;
                    OnPropertyChanged("Suffix");
                    OnPropertyChanged("FullName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's familiar or shortened name
        /// </summary>
        public string NickName
        {
            get { return nickName; }
            set
            {
                if (nickName != value)
                {
                    nickName = value;
                    OnPropertyChanged("NickName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's name carried before marriage
        /// </summary>
        public string MarriedName
        {
            get { return marriedName; }
            set
            {
                if (marriedName != value)
                {
                    marriedName = value;
                    OnPropertyChanged("MarriedName");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's gender
        /// </summary>
        public Gender Gender
        {
            get { return gender; }
            set
            {
                if (gender != value)
                {
                    gender = value;
                    OnPropertyChanged("Gender");
                }
            }
        }

        /// <summary>
        /// The age of the person.
        /// </summary>
        public int? Age
        {
            get
            {
                if (this.BirthDate == null)
                    return null;
                    
                // Determine the age of the person based on just the year.
                DateTime startDate = this.BirthDate.Value;
                DateTime endDate = (this.IsLiving || this.DeathDate == null) ? DateTime.Now : this.DeathDate.Value;
                int age = endDate.Year - startDate.Year;

                // Compensate for the month and day of month (if they have not had a birthday this year).
                if (endDate.Month < startDate.Month ||
                    (endDate.Month == startDate.Month && endDate.Day < startDate.Day))
                    age--;

                return Math.Max(0, age);
            }
        }

        /// <summary>
        /// The age of the person.
        /// </summary>
        [XmlIgnore]
        public AgeGroup AgeGroup
        {
            get
            {
                AgeGroup ageGroup = AgeGroup.Unknown;

                if (this.Age.HasValue)
                {
                    // The AgeGroup enumeration is defined later in this file. It is up to the Person
                    // class to define the ages that fall into the particular age groups
                    if (this.Age >= 0 && this.Age < 20)
                        ageGroup = AgeGroup.Youth;
                    else if (this.Age >= 20 && this.Age < 40)
                        ageGroup = AgeGroup.Adult;
                    else if (this.Age >= 40 && this.Age < 65)
                        ageGroup = AgeGroup.MiddleAge;
                    else
                        ageGroup = AgeGroup.Senior;
                }
                return ageGroup;
            }
        }

        /// <summary>
        /// The year the person was born
        /// </summary>
        public string YearOfBirth
        {
            get
            {
                if (birthDate.HasValue)
                    return birthDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                else
                    return "-";
            }
        }

        /// <summary>
        /// The year the person died
        /// </summary>
        public string YearOfDeath
        {
            get
            {
                if (deathDate.HasValue && !isLiving)
                    return deathDate.Value.Year.ToString(CultureInfo.CurrentCulture);
                else
                    return "-";
            }
        }

        /// <summary>
        /// Gets or sets the person's birth date.  This property can be null.
        /// </summary>
        public DateTime? BirthDate
        {
            get { return birthDate; }
            set
            {
                if (birthDate == null || birthDate != value)
                {
                    birthDate = value;
                    OnPropertyChanged("BirthDate");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("AgeGroup");
                    OnPropertyChanged("YearOfBirth");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's place of birth
        /// </summary>
        public string BirthPlace
        {
            get { return birthPlace; }
            set
            {
                if (birthPlace != value)
                {
                    birthPlace = value;
                    OnPropertyChanged("BirthPlace");
                }
            }
        }

        /// <summary>
        /// Gets the month and day the person was born in. This property can be null.
        /// </summary>
        [XmlIgnore]
        public string BirthMonthAndDay
        {
            get
            {
                if (birthDate == null)
                    return null;
                else
                {
                    return birthDate.Value.ToString(
                        DateTimeFormatInfo.CurrentInfo.MonthDayPattern, 
                        CultureInfo.CurrentCulture);
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's death of death.  This property can be null.
        /// </summary>
        public DateTime? DeathDate
        {
            get { return deathDate; }
            set
            {
                if (deathDate == null || deathDate != value)
                {
                    deathDate = value;
                    OnPropertyChanged("DeathDate");
                    OnPropertyChanged("Age");
                    OnPropertyChanged("YearOfDeath");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's place of death
        /// </summary>
        public string DeathPlace
        {
            get { return deathPlace; }
            set
            {
                if (deathPlace != value)
                {
                    deathPlace = value;
                    OnPropertyChanged("DeathPlace");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the person is still alive or deceased.
        /// </summary>
        public bool IsLiving
        {
            get { return isLiving; }
            set
            {
                if (isLiving != value)
                {
                    isLiving = value;
                    OnPropertyChanged("IsLiving");
                }
            }
        }

        /// <summary>
        /// Gets or sets the photos associated with the person
        /// </summary>
        public PhotoCollection Photos
        {
            get { return photos; }
        }

        /// <summary>
        /// Gets or sets the person's story file.
        /// </summary>
        public Story Story
        {
            get { return story; }
            set
            {
                if (story != value)
                {
                    story = value;
                    OnPropertyChanged("Story");
                }
            }
        }

        /// <summary>
        /// Gets or sets the person's graphical identity
        /// </summary>
        [XmlIgnore, System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public string Avatar
        {
            get
            {
                string avatar = "";

                if (photos != null && photos.Count > 0)
                {
                    foreach (Photo photo in photos)
                    {
                        if (photo.IsAvatar)
                            return photo.FullyQualifiedPath;
                    }
                }

                return avatar;
            }
            set
            {
                // This setter is used for change notification.
                OnPropertyChanged("Avatar");
                OnPropertyChanged("HasAvatar");
            }
        }

        /// <summary>
        /// Determines whether a person is deletable.
        /// </summary>
        [XmlIgnore]
        public bool IsDeletable
        {
            get
            {
                // With a few exceptions, anyone with less than 3 relationships is deletable
                if (relationships.Count < 3)
                {
                    // The person has 2 spouses. Since they connect their spouses, they are not deletable.
                    if (Spouses.Count == 2)
                        return false;

                    // The person is connecting two generations
                    if (Parents.Count == 1 && Children.Count == 1)
                        return false;

                    // The person is connecting inlaws
                    if (Parents.Count == 1 && Spouses.Count == 1)
                        return false;

                    // Anyone else with less than 3 relationships is deletable
                    return true;
                }

                // More than 3 relationships, however the relationships are from only Children. 
                if (Children.Count > 0 && Parents.Count == 0 && Siblings.Count == 0 && Spouses.Count == 0) 
                    return true;

                // More than 3 relationships. The relationships are from siblings. Deletable since siblings are connected to each other or the parent.
                if (Siblings.Count > 0 && Parents.Count >= 0 && Spouses.Count == 0 && Children.Count == 0)
                    return true;

                // This person has complicated dependencies that does not allow deletion.
                return false;
            }
        }

        /// <summary>
        /// Collections of relationship connection for the person
        /// </summary>
        public RelationshipCollection Relationships
        {
            get { return relationships; }
        }

        /// <summary>
        /// Accessor for the person's spouse(s)
        /// </summary>
        [XmlIgnore]
        public Collection<Person> Spouses
        {
            get
            {
                Collection<Person> spouses = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Spouse)
                        spouses.Add(rel.RelationTo);
                }
                return spouses;
            }
        }

        [XmlIgnore]
        public Collection<Person> CurrentSpouses
        {
            get
            {
                Collection<Person> spouses = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Spouse)
                    {
                        SpouseRelationship spouseRel = rel as SpouseRelationship;
                        if (spouseRel != null && spouseRel.SpouseModifier == SpouseModifier.Current)
                            spouses.Add(rel.RelationTo);
                    }
                }
                return spouses;
            }
        }

        [XmlIgnore]
        public Collection<Person> PreviousSpouses
        {
            get
            {
                Collection<Person> spouses = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Spouse)
                    {
                        SpouseRelationship spouseRel = rel as SpouseRelationship;
                        if (spouseRel != null && spouseRel.SpouseModifier == SpouseModifier.Former)
                            spouses.Add(rel.RelationTo);
                    }
                }
                return spouses;
            }
        }

        /// <summary>
        /// Accessor for the person's children
        /// </summary>
        [XmlIgnore]
        public Collection<Person> Children
        {
            get
            {
                Collection<Person> children = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Child)
                        children.Add(rel.RelationTo);
                }
                return children;
            }
        }

        /// <summary>
        /// Accessor for all of the person's parents
        /// </summary>
        [XmlIgnore]
        public Collection<Person> Parents
        {
            get
            {
                Collection<Person> parents = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Parent)
                        parents.Add(rel.RelationTo);
                }
                return parents;
            }
        }

        /// <summary>
        /// Accessor for the person's siblings
        /// </summary>
        [XmlIgnore]
        public Collection<Person> Siblings
        {
            get
            {
                Collection<Person> siblings = new Collection<Person>();
                foreach (Relationship rel in relationships)
                {
                    if (rel.RelationshipType == RelationshipType.Sibling)
                        siblings.Add(rel.RelationTo);
                }
                return siblings;
            }
        }

        /// <summary>
        /// Accessor for the person's half siblings. A half sibling is a person
        /// that contains one or more same parents as the person, but does not 
        /// contain all of the same parents.
        /// </summary>
        [XmlIgnore]
        public Collection<Person> HalfSiblings
        {
            get
            {
                // List that is returned.
                Collection<Person> halfSiblings = new Collection<Person>();

                // Get list of full siblings (a full sibling cannot be a half sibling).
                Collection<Person> siblings = this.Siblings;

                // Iterate through each parent, and determine if the parent's children
                // are half siblings.
                foreach (Person parent in Parents)
                {
                    foreach (Person child in parent.Children)
                    {
                        if (child != this && !siblings.Contains(child) &&
                            !halfSiblings.Contains(child))
                        {
                            halfSiblings.Add(child);
                        }
                    }
                }

                return halfSiblings;
            }
        }

        /// <summary>
        /// Get the person's parents as a ParentSet object
        /// </summary>
        [XmlIgnore]
        public ParentSet ParentSet
        {
            get
            {
                // Only need to get the parent set if there are two parents.
                if (Parents.Count == 2)
                {
                    ParentSet parentSet = new ParentSet(Parents[0], Parents[1]);
                    return parentSet;
                }
                else return null;
            }
        }

        /// <summary>
        /// Get the possible combination of parents when editting this person or adding this person's sibling.
        /// </summary>
        [XmlIgnore]
        public ParentSetCollection PossibleParentSets
        {
            get
            {
                ParentSetCollection parentSets = new ParentSetCollection();

                foreach (Person parent in Parents)
                {
                    foreach (Person spouse in parent.Spouses)
                    {
                        ParentSet ps = new ParentSet(parent, spouse);

                        // Don't add the same parent set twice.
                        if (!parentSets.Contains(ps))
                            parentSets.Add(ps);
                    }
                }

                return parentSets;
            }
        }

        /// <summary>
        /// Calculated property that returns whether the person has 2 or more parents.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public bool HasParents
        {
            get
            {
                return (Parents.Count >= 2);
            }
            set
            {
                // This setter is used for change notification.
                OnPropertyChanged("HasParents");
                OnPropertyChanged("PossibleParentSets");
            }
        }

        /// <summary>
        /// Calculated property that returns whether the person has 1 or more spouse(s).
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "value")]
        public bool HasSpouse
        {
            get
            {
                return (Spouses.Count >= 1);
            }
            set
            {
                // This setter is used for change notification.
                OnPropertyChanged("HasSpouse");
                OnPropertyChanged("Spouses");
            }
        }

        /// <summary>
        /// Claculated property that returns whether the person has an avatar photo.
        /// </summary>
        [XmlIgnore]
        public bool HasAvatar
        {
            get
            {
                if (photos != null && photos.Count > 0)
                {
                    foreach (Photo photo in photos)
                    {
                        if (photo.IsAvatar)
                            return true;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of a person object.
        /// Each new instance will be given a unique identifier.
        /// This parameterless constructor is also required for serialization.
        /// </summary>
        public Person()
        {
            this.id = Guid.NewGuid().ToString();
            this.relationships = new RelationshipCollection();
            this.photos = new PhotoCollection();
            this.firstName = Const.DefaultFirstName;
            this.isLiving = true;
        }

        /// <summary>
        /// Creates a new instance of the person class with the firstname and the lastname.
        /// </summary>
        public Person(string firstName, string lastName) : this()
        {
            // Use the first name if specified, if not, the default first name is used.
            if (!string.IsNullOrEmpty(firstName))
                this.firstName = firstName;

            this.lastName = lastName;
        }

        /// <summary>
        /// Creates a new instance of the person class with the firstname, the lastname, and gender
        /// </summary>
        public Person(string firstName, string lastName, Gender gender) : this(firstName, lastName)
        {
            this.gender = gender;
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// INotifyPropertyChanged requires a property called PropertyChanged.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Fires the event for the property when it changes.
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region IEquatable Members

        /// <summary>
        /// Determine equality between two person classes
        /// </summary>
        public bool Equals(Person other)
        {
            return (this.Id == other.Id);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the spouse relationship for the specified spouse.
        /// </summary>
        public SpouseRelationship GetSpouseRelationship(Person spouse)
        {
            foreach (Relationship relationship in this.relationships)
            {
                SpouseRelationship spouseRelationship = relationship as SpouseRelationship;
                if (spouseRelationship != null)
                {
                    if (spouseRelationship.RelationTo.Equals(spouse))
                        return spouseRelationship;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the combination of parent sets for this person and his/her spouses
        /// </summary>
        /// <returns></returns>
        public ParentSetCollection MakeParentSets()
        {
            ParentSetCollection parentSets = new ParentSetCollection();

            foreach (Person spouse in Spouses)
            {
                ParentSet ps = new ParentSet(this, spouse);

                // Don't add the same parent set twice.
                if (!parentSets.Contains(ps))
                    parentSets.Add(ps);
            }

            return parentSets;
        }

        /// <summary>
        /// Called to delete the person's photos
        /// </summary>
        public void DeletePhotos()
        {
            // Delete the person's photos
            foreach (Photo photo in this.photos)
            {
                photo.Delete();
            }
        }

        /// <summary>
        /// Called to delete the person's story
        /// </summary>
        public void DeleteStory()
        {
            if (this.story != null)
            {
                this.story.Delete();
                this.story = null;
            }
        }

        #endregion
    }

    /// <summary>
    /// Enumeration of the person's gender
    /// </summary>
    public enum Gender
    {
        Male, Female
    }

    /// <summary>
    /// Enumeration of the person's age group
    /// </summary>
    public enum AgeGroup
    {
        Unknown, Youth, Adult, MiddleAge, Senior
    }

    /// <summary>
    /// Representation for a Parent couple.  E.g. Bob and Sue
    /// </summary>
    public class ParentSet : IEquatable<ParentSet>
    {
        private Person firstParent;

        private Person secondParent;

        public Person FirstParent
        {
            get { return firstParent; }
            set { firstParent = value; }
        }

        public Person SecondParent
        {
            get { return secondParent; }
            set { secondParent = value; }
        }

        public ParentSet(Person firstParent, Person secondParent)
        {
            this.firstParent = firstParent;
            this.secondParent = secondParent;
        }

        public string Name
        {
            get
            {
                string name = "";
                name += firstParent.Name + " + " + secondParent.Name;
                return name;
            }
        }

        // Parameterless contstructor required for serialization
        public ParentSet() { }

        #region IEquatable<ParentSet> Members

        /// <summary>
        /// Determine equality between two ParentSet classes.  Note: Bob and Sue == Sue and Bob
        /// </summary>
        public bool Equals(ParentSet other)
        {
            if (other != null)
            {
                if (this.firstParent.Equals(other.firstParent) && this.secondParent.Equals(other.secondParent))
                    return true;

                if (this.firstParent.Equals(other.secondParent) && this.secondParent.Equals(other.firstParent))
                    return true;
            }

            return false;
        }

        #endregion
    }

    /// <summary>
    /// Collection of ParentSet objects.
    /// </summary>
    public class ParentSetCollection : Collection<ParentSet> { }
}
