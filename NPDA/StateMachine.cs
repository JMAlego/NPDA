using System;
using System.Linq;
using System.Collections.Generic;

namespace NPDA
{
    public class StateMachine
    {
        private HashSet<State> states;
        private HashSet<Transition> transitions;
        private HashSet<State> current_states;
        private string stack;
        private bool started;
        private int cycle_count = 0;

        public StateMachine()
        {
            if (Console.OutputEncoding != System.Text.Encoding.UTF8)
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            started = false;
            stack = "";
            current_states = new HashSet<State>();
            states = new HashSet<State>();
            transitions = new HashSet<Transition>();
        }

        public StateMachine(string initial_stack_symbol)
        {
            if (Console.OutputEncoding != System.Text.Encoding.UTF8)
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            started = false;
            stack = initial_stack_symbol;
            current_states = new HashSet<State>();
            states = new HashSet<State>();
            transitions = new HashSet<Transition>();
        }

        public StateMachine(State initial_state, string initial_stack_symbol)
        {
            if (Console.OutputEncoding != System.Text.Encoding.UTF8)
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            started = false;
            stack = initial_stack_symbol;
            current_states = new HashSet<State>();
            states = new HashSet<State>();
            transitions = new HashSet<Transition>();
            initial_state.makeInitial();
            states.Add(initial_state);
        }

        public StateMachine(State initial_state, State final_state, string initial_stack_symbol)
        {
            if (Console.OutputEncoding != System.Text.Encoding.UTF8)
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            started = false;
            stack = initial_stack_symbol;
            current_states = new HashSet<State>();
            states = new HashSet<State>();
            transitions = new HashSet<Transition>();
            if (initial_state == final_state)
            {
                initial_state.makeInitial();
                initial_state.makeFinal();
                states.Add(initial_state);
            }
            else
            {
                initial_state.makeInitial();
                states.Add(initial_state);
                final_state.makeFinal();
                states.Add(final_state);
            }
        }

        public HashSet<State> getCurrentStates()
        {
            return current_states;
        }

        public State getCurrentState()
        {
            if (current_states.Count == 1)
                return current_states.ToArray()[0];
            return null;
        }

        public string getCurrentStateValue()
        {
            return getCurrentState() == null ? "" : getCurrentState().getValue();
        }

        public State getStateById(string id_to_get)
        {
            if (id_to_get == null)
                throw new ArgumentNullException("Id to find must not be null.");

            foreach (State state in states)
            {
                if (state.matchesId(id_to_get))
                    return state;
            }
            return null;
        }

        public Transition getTransitionByState(State state_to_find)
        {
            if (state_to_find == null)
                throw new ArgumentNullException("State to find must not be null.");

            foreach (Transition trans in transitions)
            {
                if (trans.hasState(state_to_find))
                    return trans;
            }
            return null;
        }

        public void removeState(State to_remove)
        {
            if (to_remove == null)
                throw new ArgumentNullException("State to remove must not be null.");
            if (!hasState(to_remove))
                throw new ArgumentException("State to remove is not in this instance.");

            states.Remove(to_remove);

            foreach (Transition trans in transitions)
            {
                if (trans.hasState(to_remove))
                    transitions.Remove(trans);
            }
        }

        public void addState(State to_add)
        {
            if (to_add == null)
                throw new ArgumentNullException("State to add must not be null.");

            foreach (Transition trans in to_add.getTransitions())
            {
                transitions.Add(trans);
            }
            states.Add(to_add);
        }

        public void addTransition(Transition to_add)
        {
            if (to_add == null)
                throw new ArgumentNullException("Transition to add must not be null.");
            if (!states.Contains(to_add.getSource()))
                throw new ArgumentException("Transition's source must be in state machine.");
            if (!states.Contains(to_add.getDestination()))
                throw new ArgumentException("Transition's destination must be in state machine.");

            to_add.getSource().addTransition(to_add);
            to_add.getDestination().addTransition(to_add);
            transitions.Add(to_add);
        }

        public void start()
        {
            cycle_count = 0;
            current_states = new HashSet<State>();
            current_states.Add(getInitialState());
            started = true;
        }

        public void input(string inp)
        {
            if (!started)
                throw new Exception("State Machine must be started.");
            cycle_count++;
            HashSet<State> updated_current_states = new HashSet<State>();
            if (current_states.Count != 0)
            {
                foreach (State current_state in current_states)
                {
                    HashSet<State> new_current_states = current_state.transition(inp, ref stack);
                    foreach (State new_current_state in new_current_states)
                        updated_current_states.Add(new_current_state);
                }
            }
            current_states = updated_current_states;
        }

        public bool isFinished()
        {
            return started && (accepted() || current_states.Count == 0);
        }

        public bool accepted()
        {
            bool anyAccepted = false;
            HashSet<State> finalStates = getFinalStates();
            foreach (State current_state in current_states)
            {
                if (finalStates.Contains(current_state))
                {
                    anyAccepted = true;
                    break;
                }
            }
            return anyAccepted;
        }

        public HashSet<State> getFinalStates()
        {
            HashSet<State> resultant_set = new HashSet<State>();

            foreach (State state in states)
                if (state.isFinal())
                    resultant_set.Add(state);

            return resultant_set;
        }

        public State getInitialState()
        {
            foreach (State state in states)
                if (state.isInitial())
                    return state;

            return null;
        }

        public bool hasState(State search_state)
        {
            if (search_state == null)
                throw new ArgumentNullException("State to search for must not be null.");

            return states.Contains(search_state);
        }

        public bool hasTransition(Transition search_trans)
        {
            if (search_trans == null)
                throw new ArgumentNullException("Transition to search for must not be null.");

            return transitions.Contains(search_trans);
        }

        public void linkStates(State src, State dst, string cond, string sttop, string stadd)
        {
            if (!hasState(src))
                throw new ArgumentException("Source state not in state machine.");
            if (!hasState(dst))
                throw new ArgumentException("Destination state not in state machine.");

            Transition new_trans = new Transition(src, dst, cond, sttop, stadd);
            transitions.Add(new_trans);
        }

        public void linkStates(State src, State dst, string cond)
        {
            if (!hasState(src))
                throw new ArgumentException("Source state not in state machine.");
            if (!hasState(dst))
                throw new ArgumentException("Destination state not in state machine.");

            Transition new_trans = new Transition(src, dst, cond, "", "");
            transitions.Add(new_trans);
        }

        public override string ToString()
        {
            string resultant_string = "Nondeterministic Pushdown Automaton: \r\n";
            resultant_string += "  Final States: (";
            bool first = true;
            foreach (State final_state in getFinalStates())
            {
                resultant_string += (first ? "" : ", ") + final_state;
                first = false;
            }
            resultant_string += ")\r\n";
            resultant_string += "  Started: (" + started + ")\r\n";
            resultant_string += "  Input Count: (" + cycle_count + ")\r\n";
            resultant_string += "  Initial State: (" + getInitialState() + ")\r\n";
            resultant_string += "  Current State Finished: (" + isFinished() + ")\r\n";
            resultant_string += "  Current State Accepted: (" + accepted() + ")\r\n";
            resultant_string += "  Current States: (";
            first = true;
            foreach (State current_state in current_states)
            {
                resultant_string += (first ? "" : ", ") + current_state;
                first = false;
            }

            resultant_string += ")\r\n  Stack: (" + stack + ") \r\n  Transitions: {\r\n";

            foreach (Transition trans in transitions)
            {
                resultant_string += "    " + trans + "\r\n";
            }

            return resultant_string + "  }";
        }
    }
}
