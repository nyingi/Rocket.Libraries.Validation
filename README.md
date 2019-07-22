# Rocket.Libraries.Validation
A Functional C# validation library

# What It Is
This is a simple library that allows you to organize validation rules and then evaluate the rules during runtime with an exception getting thrown if any of the rules, fails.

# Why Use a Library?
Well first off, GiGo - validation can be very mundane but it is essential in all serious applications.

I found myself writing a forest of __*if-elseif-else*__ or __*switch blocks*__ to handle validation.

While these conditional blocks work, they tend to get more unreadable and more unmaintainable with the number of checks required.

Worse still while working in a team, there wasn't a consistent way of that team members were using to validate and hence checks were peppered all over code in various flavours. 

What this validation library allows you to do, is to collapse what would be branch statements into inline statements, for more manageable code.

# Package
Installation, grab the package from nuget https://www.nuget.org/packages/Rocket.Libraries.Validation/

# Example Usage

## Single Condition Validation
You may for example wish to check that an object isn't null before you proceed to use it. In such cases, it is common to write code similar to below.


'''csharp

public void DoStuff(User user)
{
  if(user == null)
  {
    throw new Exception("User was not supplied");
  }
  else
  {
    //Do stuff with the user object
  }
}
