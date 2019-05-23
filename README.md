# Rocket.Libraries.Validation
A Functional C# validation library

# What It Is
This is a simple library that allows you to organize validation rules and then evaluate the rules during runtime with an exception getting throw if any of the rules, fails.

# Why Use a Library?
Well first off, GiGo - validation can be very mundane but it is essential in all serious applications.

I found myself writing a forest of __*if-elseif-else*__ or __*switch blocks*__ to handle validation.

While these conditional blocks work, they tend to get more unreadable and more unmaintainable with the number of checks required.

Worse still while working in a team, there wasn't a consistent way of that team members were using to validate and hence checks were peppered all over code in various flavours. 

Clearly not ideal

# How It Works
Consider the following business logic, you wish to persist information about a student to a database, and the rules below must be enforced.
1. Student's date of birth must be entered.
1. Student's date of birth cannot be a future date.
1. Student must be at least 18 years old.
1. Student's admission date must be entered.
1. Student's admission date cannot be before the date of birth.
1. Student's admission date cannot be a future date.
