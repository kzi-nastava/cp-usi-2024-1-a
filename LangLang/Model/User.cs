using System;
using System.Collections.Generic;

public abstract class User
{
	private string email;
	private string password;
	private string name;
	private string surname;
	private DateTime birthDate;
	private string gender;
	private string phoneNumber;



	public User(string email, string password, string name, string surname, DateTime birthDate, string gender, string phoneNumber)
	{
		this.email = email;
		this.password = password;
		this.name = name;
		this.surname = surname; 
		this.birthDate = birthDate;
		this.gender = gender;
		this.phoneNumber = phoneNumber;
    }
}
