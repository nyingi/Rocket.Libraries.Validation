# Rocket.Libraries.Validation
A Functional C# validation library

# What It Is
This is a simple library that allows you to organize validation rules and then evaluate the rules during runtime with an exception getting thrown if any of the rules, fails.

# Why Use a Library?

I found myself writing a forest of __*if-elseif-else*__ or __*switch blocks*__ to handle validation.

While these conditional blocks work, they tend to get more unreadable and more unmaintainable with the number of checks required.

Worse still while working in a team, there wasn't a consistent way of that team members were using to validate and hence checks were peppered all over code in various flavours. 

What this validation library allows you to do, is to collapse what would be branch statements into inline statements, for more manageable code.

# Package
Installation, grab the package from nuget https://www.nuget.org/packages/Rocket.Libraries.Validation/

# Example Usage

## Single Condition Evaluation
You may for example wish to check that an object isn't null before you proceed to use it. In such cases, it is common to write code similar to below.

```cs

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
```

With Rocket Validation, you can reduce the complexity of the above code by re-writing it as below.

```cs

public void DoStuff(User user)
{
    new DataValidator()
        .EvaluateImmediate(() => user == null, "User was not supplied");
    //Do stuff with the user object
}
```

The inline __*EvaluateImmediate*__ method takes in a boolean function as its first parameter, which if evaluated to true, causes an exception to be thrown. The thrown exceptions's message is the string passed as the second parameter to __*EvaluateImmediate*__

The second code snippet that uses the library is more readable, enforces more structure and introduces less code complexity.

## Multiple/Chained Condition Evaluation
In this example, we'll check that a __*User*__ object is not null and that the property __*Username*__ is not empty and that the length of the *__Username__* property is not less than 6 characters. Without the library, we can handle that validation as shown below.
```cs
public bool ValidateUserName(User user)
{
  const int minLength = 6;
  if(user != null)
  {
    if(!string.IsNullOrEmpty(user.Username))
    {
      if(user.Username.length < minLength)
      {
        throw new Exception($"Username '{user.Username}' is too short. At least {minLength} characters are required");
      }
      else
      {
        return true;
      }
    }
    else
    {
      throw new Exception("Username was not supplied. Usernames are mandatory");
    }
  }
  else
  {
    throw new Exception("No user was supplied");
  }
 }
}
```

With the validation library, we're able collapse the above code to something similar to the following:

```cs
public bool ValidateUserName(User user)
{
 const int minLength = 6;
 new DataValidator()
    .AddFailureCondition(() => user == null, "User was not supplied", true)
    .AddFailureCondition(() => string.IsNullOrEmpty(user.Username), "Username was not supplied. Usernames are mandatory", false)
    .AddFailureCondition(() => user.Username.Length < minLength, $"Username '{user.Username}' is too short. At least {minLength} characters are required", false)
    .ThrowExceptionOnInvalidRules();
}
```

The *__AddFailureCondition__* method also take in a ``` Func<bool>``` as its first parameter, which should evaluate to true to indicate a failing validation. As a second paramter, it takes in a ``` string ``` which serves as the error message for the failing condition, and finally it takes a ``` bool ``` parameter to indicate whether or not a rule is critical, i.e does the failing of a rule make validation of follow-up rules unnecessary.

An example of a critical rule is whether our ``` user ``` object is null, as if the object is indeed null, then it follows that trying to check whether ``` user.Username ``` is empty would result in a ``` NullPointerException ``` being thrown by the runtime.

Again, using the library greatly reduces code complexity and also as an bonus inlining allows error messages to live next to the rule they belong to, as opposed to the snippet without the library that has rule in the ``` if ``` part of the clause and the error message in the ``` else ```. This increases readabililty and maintainability.


# Validation Results
## IncorrectDataException 
__ONLY__ in cases where a validation rules fails, whether it be via the single rule method __*EvaluateImmediate*__ or the chained rule method __*AddFailureCondition*__ (or its async relative __*AddFailureConditionAsync*__) is an exception of type *__IncorrectDataException__* is thrown. If no rule(s) fail, then no exception is thrown.

The *__IncorrectDataException__* is a simple sub-class of the *__Exception__* object, with the main addition being the property *__Errors__*, which is a list that contains all the error messages from the failing rule(s).



  
