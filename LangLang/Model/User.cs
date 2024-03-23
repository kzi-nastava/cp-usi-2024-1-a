using Consts;
using System;
using System.Collections.Generic;

public abstract class User
{
	private string email;
	private string password;
	private string name;
	private string surname;
	private DateTime birthDate;
	private Gender gender;
	private string phoneNumber;

	public string Email { get; set; }
	public string Password { get; set; }	
	public string Name { get; set; }
    public string Surname { get; set; }
    public DateTime BirthDate { get; set; }
	public Gender Gender { get; set; }
	public string PhoneNumber { get; set; }

    public User(string email, string password, string name, string surname, DateTime birthDate, Gender gender, string phoneNumber)
	{
		Email = email;
		Password = password;
		Name = name;
		Surname = surname; 
		BirthDate = birthDate;
		Gender = gender;
		PhoneNumber = phoneNumber;
    }
}
