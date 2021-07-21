# Builder Pattern, IoC + AOP
>Reinvent the wheel about IoC container and AOP

## Builder Pattern
The builder pattern is a design pattern designed to provide a flexible solution to various object creation problems in object-oriented programming. 
The intent of the Builder design pattern is to separate the construction of a complex object from its representation. 
It is one of the Gang of Four design patterns.

**Method 1**

>Vanilla method using overloaded constructors

**Method 2**

>Factory design pattern

**Method 3**

>Builder design pattern

## Chain of Responsibility
How to build middleware pipeline

## Circular Reference
ChickenService and EggService is using constructor dependency injection
This will cause throwing runtime error: circular reference.

**Question**: How to create IoC container to enable other DI other than via constructor?

## MyIoC

**MyContainerV0**

0. create instance, with 1 layer of constuctor DI

**MyContainerV1**
1. multiple parameters in the constructor

2. multiple constructors (IServiceCollection will choose the super constructor, with the most parameters)

>Use Attribute to mark the desired constructor, e.g. BService

3. DI by Property, DI by method (IServiceCollection only support constructor DI)

>Property attribute or method attribute

4. how to deal with 1 interface with several implementations?

5. how to pass a const value to constructor?

**MyContainerV2**

6. life cycle support

>`AddTransient()`

>`AddSingleton()` --- singleton across all requests

>`AddScoped()` --- basically singlton per request, singleton in child container

>`AddPerThread()` --- (This is not well implemented.)

7. IoC + AOP
