using System.Collections.Generic;

namespace NPDA
{
    public class State
    {
        private HashSet<Transition> transitions;
        private string identification;
        private string value;
        private bool initial;
        private bool final;

        public State(State to_copy)
        {
            transitions = new HashSet<Transition>();
            identification = to_copy.identification;
            value = to_copy.value;
            initial = to_copy.initial;
            final = to_copy.final;
            foreach (Transition trans in to_copy.transitions)
                transitions.Add(new Transition(trans));
        }

        public State(string id)
        {
            transitions = new HashSet<Transition>();
            identification = id;
            value = "";
            initial = false;
            final = false;
        }

        public State(string id, string val)
        {
            transitions = new HashSet<Transition>();
            identification = id;
            value = val;
            initial = false;
            final = false;
        }

        public State(string id, string val, bool is_initial, bool is_final)
        {
            transitions = new HashSet<Transition>();
            identification = id;
            value = val;
            initial = is_initial;
            final = is_final;
        }

        public bool matchesId(string id_to_match)
        {
            return id_to_match == identification;
        }

        public bool isInitial()
        {
            return initial;
        }

        public bool isFinal()
        {
            return final;
        }

        public State makeInitial()
        {
            initial = true;
            return this;
        }

        public State makeFinal()
        {
            final = true;
            return this;
        }

        public string getValue()
        {
            return value;
        }

        public State addTransition(Transition trans)
        {
            transitions.Add(trans);
            return this;
        }

        public HashSet<State> transition(string input, ref string stack)
        {
            HashSet<State> to_return = new HashSet<State>();
            foreach (Transition trans in transitions)
            {
                if (trans.canTransition(this, input, ref stack))
                    to_return.Add(trans.traverse(ref stack));
            }

            return to_return;
        }

        public HashSet<Transition> getTransitions()
        {
            return transitions;
        }

        public bool hasTransition(Transition search_trans)
        {
            foreach (Transition trans in transitions)
            {
                if (trans == search_trans)
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return identification;
        }
    }
}
