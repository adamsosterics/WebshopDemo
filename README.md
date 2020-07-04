WebshopDemo
===========

It is a demo application to show how I would build a modular monolith. It needs a lot more polish to be a really usable and deployable app, but I think the most important things (from an architectural viewpoint) are there.

## Principles

These are the principles I am trying to follow during the development:

 - Domain-driven design
   - I divided the codebase into different bounded context in order to achieve high cohesion and low coupling. I'm using entities and value objects to create my aggregates.
 - Automated testing
   - It's important that we test our code. I'm using a mix of unit and integration tests to check my code, but in a real project there should be other types of tests (end-to-end, performance, ...). The unit tests and some of the integration tests are written with test-first approach.
 - Ports and adapters, clean architecture
   - It's not a by-the-book implementation of either of these architectural styles, but the gist of them are there: separate the inside and the outside of the application, make the use cases first-class citizens, make sure the dependencies point the right way.
 - CQRS
   - It isn't pushed to its logical conclusion, but I do have different objects to modify and to query the state of the application.
 - Clean code
   - I'm trying to make my code readable for human beings, I hope I succeeded :)
 - Countless others
   - Like SOLID principles, KISS, DRY (though I've created multiple Price DTOs), ...

## What's missing?

A lot. Logging, exception handling, transaction handling, end-to-end tests, BDD scenarios, automated build scripts, DB creation scripts, authentication, authorization, validation, security, frontend design, just to name a few.

## Features

 - Registering new products
 - Setting their price
 - Adding items to cart
 - Removing items from cart
 - Marking items in cart that had their price changed
 