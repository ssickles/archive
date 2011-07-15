package com.IdentityStream;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.PrintWriter;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import IdentityStream.DataModels.Template;

/**
 * Servlet implementation class HelloWorldServlet
 */
public class HelloWorldServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;
       
    /**
     * @see HttpServlet#HttpServlet()
     */
    public HelloWorldServlet() {
        super();
    }

	/**
	 * @see HttpServlet#doGet(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		PrintWriter writer = response.getWriter();
		writer.append("Hello World");
		writer.flush();
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		ObjectInputStream inputFromApplet = null;
		Template template = null;
		
		try
		{
			inputFromApplet = new ObjectInputStream(request.getInputStream());
			template = (Template)inputFromApplet.readObject();
			inputFromApplet.close();
		}
		catch (Exception e)
		{
			System.out.println("Unable to receive the template from the applet.\n");
			e.printStackTrace();
		}
		
		System.out.println("Template received, now verifying.\n");
		System.out.println("Template unit: " + template.getUnitCode() + "\n");
		
		try
		{
			PrintWriter writer = response.getWriter();
			writer.append("Verification passed.\n");
			writer.flush();
			writer.close();
		}
		catch (Exception e)
		{
			System.out.println("Unable to send response.\n");
			e.printStackTrace();
		}
		
	}

}
