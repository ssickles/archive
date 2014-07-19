﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AtmTest
{
    class Program
    {
        static void Main(string[] args)
        {
            atm.AtmIntegrationServiceClient proxy = new atm.AtmIntegrationServiceClient();
            //working template from pos
            //int result = proxy.verifyCustomer("1234567890123456", Convert.FromBase64String("Rk1SACAyMAAA7AAdAAAAAAEAAZAAxQDFAQABAGEgAJsAIhEAANoANWgAAF0AQ3YAAKYARW4AAD0AVnEAAKYAWhQAAEQAchkAAJgAjBQAAEEAjnEAAEsAtHEAADEAvXMAAD0AxBkAALIAxhcAAEQA23YAAMcA4BQAAO8A5w4AAKIBA3YAANwBFHEAAKIBInYAAIgBKx8AAM4BKxwAADEBMH8AAO8BMnMAANoBOXMAAJEBPB8AAOMBQxwAAM4BRRwAAOEBVnYAAOMBWhkAAGABXyUAAFIBaC0AAJEBb2sAAAwKAQAMaAWIAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=="));
            //ncr template not good
            //int result = proxy.verifyCustomer("1234567890123456", Convert.FromBase64String("Rk1SACAyMAABKAAdAAAAAAGgAaAAxQDFAQABAAAqAI0ANgsAAFsAPxcAALwAQQAAAGwASBQAAHoAS3EAANEAS7EAAOMAS1QAAHcAcHEAALQAgbEAALkAjWgAAQQAlJsAAO8AmJsAANoAn5UAALwApIQAAMAAq4oAAM4AsooAADMAuX8AAHUAwycAAI8Ax4cAAGAAyoQAAEsAzCUAAKYAzpUAANgAznwAALcA0bEAAHoA04cAAOYA04QAARUA2I8AAI8A2o0AAP0A44EAAKIA5psAANEA6FcAAQ4A64cAAPkA9icAANEA+1oAAOgBB2sAAMwBDloAAPsBMQ4AALwBPVQAANwBPQYAAJ8BXlQAAMUBYq4AALcBcFcAAAwKAQAIUAVoAAAAAAA="));
            //neuro template of ncr person
            //int result = proxy.verifyCustomer("1234567890123456", Convert.FromBase64String("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//////8AAxIGDmX///////8ECisHBVMA/0//////AfL///8ABCUFBlQC/y////8JCFcLBbIECysHDSMGAyUAAmP///8BA2QFByMMDcT/BvMFACgCClgLDaEXFFUKBzcE/38J/z8ZFDgIBzYE/1////8UFiMLBzgE/58IGTQVDBENB3sCBNsKFDISECUGB8kLChQXFlIHDBcREyEhD3P/BvQGDyMTIXP///////////8O/z8GDTMTHkQeEXENBjgHDFUSGDIeE1IPDVIHEEESGEMcGEIQBjoHDHIWFSEh/48PDUEGEWIYHmMbFhEMCiII/18XHSQbHDYYEEUSDBEKFmEVDBILCBYUGhkbHj0bGCgUCiEIGWUaHVIeH5n/EfMQEiIWG0IaHSUXCFEJ/z////8iHSMXCVQZ/y////8YFhMXGicdIkkcIT4YFiYbHTIiJnsgIXwcGyQXGiP///8iIFUmI1Uf/x8PGEkcIHETGDkeIBElI0Uh/18eGBkcInEnJkUlH0EPE0gfJFL///////////8nJkUgHBcd/18hExgfJVEo/z8k/x8o/y////8hEykfI2EjHxQgJ0Em/x8oJCH/////KPIlIBUiJ1EmHBoi/0//////KPQkIyMlJiIn/y/////oK/KZwBcKlOjZQBMIhgXdwB0JnGLlIBUKjxMN4R8Nn+t1oRYJp1+BwRQEqmONgRcLqCpZoiIPih9ZwiUJhC5lwh8ViDV1Qh4Yicq6YhMElkndQhAIlczlwg0FlDzlAhYHk7sGIxw7jD4RYxMEmjoSAxUJoDAaQxkRkE8hwx8SigQyAxszj4Q6Yx0pj0VJ4yAOhyxRgxcOrTdhIyYFhj+pAyULiX29Yx0ni4MZhBwZm2wZRCAUc/G9ZCIOeI3RBBoHkKTdBA8Tf/vhxB4OgIklJRMJhQgpZREGgIg6JRYJhAdpZRoEgYBqhR4LgYJ6pRgMf4KWZRUGfXjCpRoIffvhxRcIdQQCAgs+BQNR////////AAMRBg5U////////BAoaBwNCAP8//////wHx////AAQTBQZDCQhXBwA0Av8f////BAcRDwxjBgEWAwJC////AQNTBQcSDwx0/wXxA/9vAgtHEwxyFxRGCgclBP9/Cf8vGRcmCghSBP9f////FBAUCwcWBP+fCBkjEBUjDAe3BAqhCBdBBwsrDxExIA1z/wb0/w7y/wH3BgwTESAzBg0SGCDC////////EhESDAY3BwtHExgxCwgnFBYyHBWCDwdaIP9/Df8/DA8SEhwkIP+vEQwjBw9BExhTHBhCEg9TBwt0EBUyGxYSBwuxCBlVFx0TFhsTJBjkEwcpEBQjFRASCxQiFxtDHBhkGxgXFAsRCP9PGh0xHyB9/xLzDxMSFRtBGh0TFwhBCf8v////Hh0SFwgxGf8f////GBUTFhcxGh1THCItEhU2Gx0hHiFmHyNrHBsTFxoS////Hh9E/////yb1IR0UGv8fDRhXHCFhJiVEJCJEDhEnHyNB////////////JiVEHxwWHR5BIBEXHCShJygj/yPxKP8v////IBEYHCKxIhgcHyZBJyokKCMx/ynyKichJB8UISZBJR8UIf9P/////ynzJBgtHyVR/ynyKighIyIjJCcxKv8v////KCciJSYj/////yrx/////////yjyJykhzi34kUAYDpbhqgASCIHq0YATB4oI4QAeCqFh+kAVCpGSOoEhCZbtggEXC6dgocEWC6otaQIjDY4ncUImDIFEgYIbUo8vgQIgEovMocITBZ86ooIcJIw5skIeDY3MyQIOBZxI4sIPCZhA8QIWCZY+EkMVDJ4qIYMZFZFAKYMTBZ1OKgMfFo8QMsMaN5F4OoMdJpExQsMXDqNIWcMgD43BekMkB5NAqoMlC5CQ0YMcLpKIMoQcF5xwMYQfEXrtucQiEXGq0UQOEHuQ0YQZCZf80gQfDYGTIYURBoAcIUUSB4SQMYUVCYmJWsURC4AIcQUaB5CCckUeBoaKmsUaCJF4sYUbCIsB4kUaBomB+gUWDH4cAgMNTgYEYgH/P////wACMgYPhv///////wAEEgcPhf8B8v///wUIKQYEMgD/T/////8C8v8A8gMFIwYMRwP/L////wkIVwoGwQUKGQcME/8C9wQDQ////wQGYQMLWAoMkhkVRQsHNwX/fwn/PxsaNAgHNgX/X////xMRJf8H+QULtA0XExUOIg0HOAX/nwgaMgcKJxESMiAQhP8E+BYTEgwKgQULsw4XERcWEw0HGQULov8V8QcMRhAgI/////8B9v///w8BKAcMNBQgQxIUExAMYwcKVRMYQiAUohAMUhEKFhgdMxwlLhgRJAcNghcWISD/jxABTAwSIhghZRwXEQ0OIQj/XxkeJB4cYyQY4xMNEQ4XQRYKEw4VERkeRhwkPiEgnhQRYgcTYhYcMhwYNxUOIgj/TxoeMgn/T/8b8R4cNRkIMR8eExkaUQn/P////xMXIxUaFh4dQyX/7xIcMxkeIR8iZSEjax0cFBkbI/8f9SIhRf////8o9SIeFRobQRAUOCEjUf///////xQcWh0iYSgnRSUkRf///ygnRSEdFR4fQSAUGB0kwSwmYf///ywmUf8j8RAUWCElUSAYHSEoQScsJ/8k8SAjIiQlEiz/T////yopQSwmUSUhJSIoUSEdSSL/T/////8q8iUnMSIoYSorEiwmIiQpQSIocv////8r8f////8s8SYnVCooEv8l9ycrUf///////8ws9JkAGAyV4qJAEgeB6dlAEwiIAenAHQqfYgIBFQmRDxnBHw+k6YGBFgmo8IlBEwaqYZkBFwypLGHCIg2MHmkCJgqCLnHCHxOHVJkCCweczKKCEwSfRKrCHBuNPLpCHg2OztHCDQWbSOrCDwmXPukCFgiUQhGDEwSZPBoDFQugMBmDGRSPUDIDHxeOCjoDG0KSRkHDJAiKMErDFw6likoDHTmQSlEDIQ6NPLJDJQuPfcFDHSiMcCkEIBN1jDIEHB2g8MFEIg91/NJEHw2BkNnEGQiUouEEDxN6iTEFFgmJBnHFGQiQhHLFHQWIgIIFGQqQfJrFFQmJhLGFFwuMfrkFHAeG/uEFGAmIEAIDCz0GBFIB/z////8AAzYCDCP///////8ABBIHDEL///8B/y8FCxoOBNIA/z//////AvIBAFIDBSMGB0IKCVcNCHMD/x////8FCBESDWQH/z8EA0IGDk4NDHT///8CBEIMB3MGABgDC0YODbIYDxULCCUF/38K/y//GPMPCYIF/1////8WDyISCIcF/58JGCL/////AfMHDTkQIzMIDikSEzIREEX/B/cVEjYHCOoFC8MPFiMaFyQOCCkLCSUYFlEHDRYRIyL///8MATb///8QASkHDUUTI0QUExIRDWMIDkYVGTIj/38RDUIIEiIUIiQj/68TDSMSDhgZH0MfGVIUElMIDnMWFzEkGuIODzEJGFUbHSEaHRMkGeQVCBkOFjMeGyMPCVEK/z//HPEiI54RFLQIFWIXHUIXDhIPFiIcHZMiGdQdGhQWDyIKGGMcHkEgHiMbGEEK/0////8ZFyMaGzEcHmMfIz4fHSMbHBP/IPQhIkQZGjcdHjEhJWoiI2v/////JvUhHhQc/y////8mJVUiHxYeIEETGUkfIWEmJUQkIzQQEyciJEH///////8jGR0iITQnKCP///8q/x8pJzEkIhQhJlEiH0oh/1////8qJSEkGS4iJVEq/y8rKTL/I/MkJzMpKxP///8oJBMnKiH/K/H///8nJSEm/y//////K/H/////////KfEnKjE="));
            //ansi template of ncr person working
            int result = proxy.verifyCustomer("NCR1234567890", Convert.FromBase64String("Rk1SACAyMAAFXQAxAQAAAAGgAaAAxQDFAQAAAGQpQMEAJAcAQJ4AMxAAQO8AOLAAQKcAPG4AgQ8ATk0AQKQAYnEAQRcAmJQAQTEAmZcAgOwAmlkAgOIAnIYAQQAAnJIAgPAApY4AgOUApokAgJgAtSUAQG8AtiUAQLEAuYgAQIEAuoMAgMgAwJEAgKsAwYcAgPkAxX8AQJwAxocAgNQAx6UAQOAAxzUAgO4AymMAQQcA1YAAQL8A2JUAQTMA3IsAQSkA6oQAgO4A8GAAgOgBCVgAQP0BCWUAQM4BNE8AQPsBNwMAQJQBR1IAQJABTK4AgLUBUFMAgPcBWlkAQNgBW7EAgMsBYVUAQLEBaFsAgLoBcbMABEcAAQPdAgEAAAEAAAEAAAEAAAEEAQEGBAEKDQEDAwIAAAIAAAIAAAIAAAIRBwIGBQIEAQIBAAMAAAMAAAMAAAMBAwMEAgMSCgMJCwMFAQQGAwQKDQQFAwQDAgQBAQQAAAQCAQQAAAUEAwUJCQUHBgUIBAUAAAUAAAUAAAUDAQYOAwYKCwYJCgYDAgYEAwYCBQYAAAYAAAcAAAcIAgcAAAcFBgcGCQcLAgcUBAcZBAgAAAgAAAgAAAgFBAgDBggHAggUCAgbAgkKAAkNAAkMAAkLAQkHAwkFCQkDCwkGCgoNAAoMAAoJAAoFCgoEDQoGCwoQBAoXAQsbBAsHAgsAAAsFCAsGCgsJAQsMAQsUAQwUAAwIBwwLAQwFCQwJAAwNAAwWAwwYAA0UAQ0bBw0MAA0JAA0KAA0AAA0SAg0XAA4GAw4AAA4RAg4AAA4VAA4TAQ4QAg4BBw8AAA8AAA8AAA8AAA8iBA8VBg8RAg8GARAaARASAhANBRAFCRAGBxAOAhAVAhATABEiBBEVAxEOAhEGAhEAABEPAhEAABEAABIWABIXARINAhIEDBIGCRIQAhIAABIaARMaAhMWBRMSAxMQABMGBhMOARMVARMiCRQdABQZARQAABQHBBQMABQNARQYABQgChUgBBUaBRUTARUGBRUOABURAxUAABUiBxYXABYMAxYNARYSABYQBBYaAxYgDBYdAhcGCxcWABcaBRceBxcdBBcYARcLBRcNABgiDRgdAhgcBxgZAxgUABgMABgGDRgXARkfARkcAxkbBBkHBBkMARkUARkaBhkdABodARoXBRoSARoQARoTAhoAABoiDRogCBsAABsAABsAABsIAhsHABsZBBsfAxscARwAABwAABwAABwbARwHARwZAxwfAhwhAh0iDR0eAR0fAh0cBR0ZAB0YAh0XBB0aAR4iCh4gBR4hBR4AAB4fAB4dAR4aAR4VAx8gBB8hBB8AAB8AAB8cAh8ZAR8dAh8eACAiBCAkAyAmBCAlAyAhACAeBSAaCCAVBCEAACEfBCEeBSEgACEmBCElAyEAACEAACIAACIjACIAACIoAyIkACIeCiIVByIAACMkACMiACMVCCMAACMAACMAACMAACMoAiQjACQAACQoAiQnACQlACQgAyQaDCQiACUmACUAACUAACUAACUAACUhAyUeCCUgAyYlACYhBCYgBCYkACYnACYAACYAACYAACcoACcpACcAACcAACcmACchBCcgBCckACgAACgAACgpACgAACgnACgkAigiAygjAikmASknACkkAikoACkAACkAACkAACkAAAACABBCAOMAmIUA4ADFLEABAQAtCwgJCQgEDQoyXBESHgQDBwcNCBEHGD0cDw4ECyYUEQcOCQkHBAMIBgUBAgAtlomdj5SsjIWJiYuMipKWkpWMnIycjYyPiK2JioeXepGBhYKBgIJ/fHk="));
            Console.WriteLine("Result is: " + result);
            Console.ReadLine();
        }
    }
}