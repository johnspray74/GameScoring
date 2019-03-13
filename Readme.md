# Game Scoring project example for ALA
(ALA = Abstraction Layered Architecture)

This is one of the demonstration projects for the ALA  documentation at http://abstractionlayeredarchitecture.com.

The project implements a game scoring application for Tenpin Bowling using ALA, and then demonstrates 'maintainability' by implementing a Tennis scoring application using the same abstractions, just a different diagram.

## Getting Started

1. Read some of http://abstractionlayeredarchitecture.com to get the insight and structure of an ALA program. (The web site is organised in passes that go into more depth. Each pass ends with an example project. This Bowling and Tennis applications are the examples at the end of passes 2 and 4. 

2. Here are the ALA diagrams for these two applications:

![Bowling](/Application/BowlingDiagram.png)

![Tennis](/Application/TennisDiagram.png)

These diagrams can be viewed by opening in a new tab.

3. Given that these use ALA structure, you will appreciate that these diagrams by themselves:

* descibe all the requirements of their respective applications
* are the source code for their respective applications
* are complete executables (except the square boxes in the diagrams need small application level functions ideally shown in the box)
* the ALA architectural designs (domain abstractions and programming paradigms)
* contain a set of domain abstractions and programming paradigms that are useful for implementing other applications in the same domain.

To understand these diagrams better, refer to their step-by-step development process at the end of passes 2 and 4 at http://abstractionlayeredarchitecture.com.

4. The diagrams are manually translated into code in Tennis.cs and Bowling.cs (in the Application folder). Inspect the code to see how they directly reflect the ALA diagrams. 

Optional:

5. Read the code inside the domain abstractions (in the DomainAbstractions folder) to see how they work internally.

6. Install Visual Studio for C# community edition.

7. Clone this git repository, and double click on the solution file.

8. To see that the diagrams above actually execute, run the console application by pressing F5 in Visual Studio. Change to the other application by uncemmenting where the repsective main function in Tennis.cs or Bowling.cs.

9. Run the scoring engine tests in the Test Explorer.

## Authors

* **John Spray** 

## License


## Acknowledgments

* Robert Martin provided inspiration for the idea of using Tenpin Bowling scoring as a pedagogical sized project or Kata. Here it is used as an example project for ALA. However, this project implements a scorecard as you would see in a real bowling game, complete with individual throw scoring and an ASCII form of a scorecard in a console application. Most internet examples of Tenpin bowling you will see just return the total score.
