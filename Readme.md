# Game Scoring project example for ALA
(ALA = Abstraction Layered Architecture)

This is one of the demonstration projects for the ALA  documentation at http://abstractionlayeredarchitecture.com.

The project implements a game scoring application for Tenpin Bowling using ALA, and then uses the domain abstractions to demonstrate 'maintainability' by implementing a Tennis scoring application.

## Getting Started

1. Read some of http://abstractionlayeredarchitecture.com until you get the insights behind Abstraction Layered Architecture. The web site is organised in passes that go into more depth. Each pass ends with an example project. The Bowling and Tennis applications here are used as the examples at the end of passes 2 and 4. 

2. Here are the ALA diagrams for these two applications:

![Bowling](/Application/BowlingDiagram.png)

![Tennis](/Application/TennisDiagram.png)

These diagrams can be viewed by clicking on them in the Application folder.

3. When you understand ALA, you will understand that these diagrams by themselves represent:

* All the requirements of their respective applications
* The source code for their respective applications
* The ALA architectural designs including domain abstractions and programming paradigms for their respective applciations.
* A set of domain abstractions and programming paradigms useful for implementing other application in the same domain.

To understand these diagrams better, refer to their step-by-step development process in the examples at the end of passes 2 and 4 at http://abstractionlayeredarchitecture.com.

4. The diagrams are manually translated into code in Tennis.cs and Bowling.cs (in the Application folder). Inspect the code to see how they directly reflect the ALA diagrams. 

Optional:

5. Read the code inside the domain abstractions (in the DomainAbstractions folder) to see how they work internally.

6. Install Visual Studio for C# community edition.

7. Clone this git repsoitory, and double click on the solution file.

8. To see that those diagrams above actually execute, run the console application by pressing F5 in Visual Studio. Change to the other application by modifying the comment in Start.cs.

9. Run the tests in the Test Explorer.

## Authors

* **John Spray** 

## License


## Acknowledgments

* Robert Martin provided inspiration for the idea of using Tenpin Bowling scoring as a pedagogical sized project or Kata. Here it is used as an example project for ALA. However, this project implements a scorecard as you would see in a real bowling game, complete with individual throw scoring and an ASCII form of a scorecard in a console application. Most internet examples of Tenpin bowling you will see just return the total score.
