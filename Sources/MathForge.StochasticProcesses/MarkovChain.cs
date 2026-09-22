namespace MathForge.Stochastic;

public sealed class MarkovChain
{
	public TransitionMatrix Transitions { get; }

	public MarkovChain(TransitionMatrix transitions)
	{
		Transitions = transitions;
	}

	public int NextState(int state, IRandomGenerator random)
	{
		...
	}
}