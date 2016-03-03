# ddd_es
> WORK IN PROGRESS

A small proof of concept C# project for Domain Driven Design and Event Sourcing.
It was heavily influenced by the code here: http://www.jayway.com/2013/03/08/aggregates-event-sourcing-distilled/

Source was used as a demo for a talk: https://sway.com/QjcOHMKGvTUQ2qNT

## Points of Interest
- Uses CQRS so class for handling commands and read-only classes for querying
- A single method for handling commands. Uses reflection which reduces amount of boilerplate code needed
- Handling or commands and processing events does not use a giant if or case statement which is generally error prone (again due to reflection)
- Reflection uses a marker interface on commands to know what aggregate root to target: `interface ITargetAggregate<T> where T : IAmAggregate`
- `Handle` method on domain objects process commands. This is where validation is done. The `Handle` method returns a list of events that the domain raises due to that command. 
When events are processed by the `Process` method, there is no validation since these events have already happened and so must be processed.

