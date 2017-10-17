# NPDA

### Description

NPDA is a simple implementation of a non-deterministic pushdown automaton in C Sharp. It's not perfect and doesn't pretend to be, but it's all I needed to work with them easily in C Sharp. And hey, it was made in three days of on and off work.

***FYI: It's not done yet. It can't process lambda transitions properly yet and isn't fully tested***

### Classes

NPDA contains three classes:
  - State
  - Transition
  - StateMachine

#### State

States simply describe a state in an NPDA, it has transitions, an ID and a value which is useful for actually code using it, if not strictly part of an NPDA.

#### Transition

Transitions, again, simply describe a transition in an NPDA, they have source and destination states, a condition, a stack condition, and a stack removal/replacement.

#### StateMachine

StateMachines have a set of states and transitions and manage the interrelation of them, they also manage the input and actual running of the NPDA represented by them.