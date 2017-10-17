namespace NPDA
{
    public class Transition
    {
        private State source;
        private State destination;
        private string condition = "";
        private string stack_top = "";
        private string stack_add = "";

        public Transition(Transition to_copy)
        {
            source = to_copy.source;
            destination = to_copy.destination;
            condition = to_copy.condition;
            stack_top = to_copy.stack_top;
            stack_add = to_copy.stack_add;
        }

        public Transition(State src, State dst, string cond, string sttop, string stadd)
        {
            source = src;
            destination = dst;
            condition = cond;
            source.addTransition(this);
            destination.addTransition(this);
            stack_top = sttop;
            stack_add = stadd;
        }

        public bool hasState(State state)
        {
            return state == source || state == destination;
        }

        public bool isSource(State state)
        {
            return state == source;
        }

        public bool isDestination(State state)
        {
            return state == destination;
        }

        public bool canTransition(State current_state, string cond, ref string stack)
        {
            return cond == condition && isSource(current_state) && stack.EndsWith(stack_top);
        }

        public State traverse(ref string stack)
        {
            stack = stack.Substring(0, stack.Length - stack_top.Length);
            stack += stack_add;
            return destination;
        }

        public State getSource()
        {
            return source;
        }

        public State getDestination()
        {
            return destination;
        }

        public override string ToString()
        {
            return source + " -(" + condition + "," + (stack_top == "" ? "λ" : stack_top) + "/" + (stack_add == "" ? "λ" : stack_add) + ")-> " + destination;
        }
    }
}
