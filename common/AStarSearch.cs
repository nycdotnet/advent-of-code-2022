using System.Numerics;

namespace common
{
    public class AStarSearch<TNode, TCost>
        where TNode : notnull
        where TCost : INumber<TCost>
    {
        // for the given key location identifier, returns the identifier of the previous location
        // along the least expensive path.
        public readonly Dictionary<TNode, TNode> cameFrom = new();

        // the least expensive known cost to get to the given key location
        public readonly Dictionary<TNode, TCost> bestKnownCostTo = new();

        /// <summary>
        /// Runs an AStar search on the given graph, optimizing the path from start to goal.
        /// The heuristic is a method that helps identify if a given traversal is improving the
        /// situation or not.  A traversal heuristic that produces a lower result is considered
        /// promising than one that is higher.
        /// </summary>
        /// <param name="heuristic">A Func which is passed the next <typeparamref name="TNode"/>
        /// and the goal <typeparamref name="TNode"/>.  An ideal heuristic returns a lower value
        /// the closer the next parameter is to the goal</param>
        public AStarSearch(IWeightedGraph<TNode, TCost> graph,
            TNode start,
            TNode goal,
            Func<TNode, TNode, TCost> heuristic,
            TCost? failIfCostExceeds = default)
        {
            Start = start;
            Goal = goal;
            var hasCostFail = failIfCostExceeds != default;

            var frontier = new PriorityQueue<TNode, TCost>();
            frontier.Enqueue(start, TCost.Zero);

            // we came from the start, so this is effectively the "identity element"
            cameFrom[start] = start;
            // there is zero cost to get here because this is the start.
            bestKnownCostTo[start] = TCost.Zero;

            while (frontier.TryDequeue(out var current, out var _))
            {
                if (current.Equals(goal))
                {
                    PathFound = true;
                    break;
                }
                var costToGetHere = bestKnownCostTo[current];
                foreach (var next in graph.GetNeighbors(current))
                {
                    var newCost = costToGetHere + graph.Cost(current, next);
                    if (hasCostFail && newCost > failIfCostExceeds!)
                    {
                        continue;
                    }
                    if (!bestKnownCostTo.TryGetValue(next, out var previousBestCost) || previousBestCost > newCost)
                    {
                        // we have never been here, or the previous best cost was more expensive.
                        bestKnownCostTo[next] = newCost;
                        var priority = newCost + heuristic(next, goal);
                        frontier.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }
        }

        public TNode Start { get; }
        public TNode Goal { get; }
        public bool PathFound { get; private set; }

        /// <summary>
        /// returns the optimal path, navigating backward from the goal to the start and not including the start.
        /// </summary>
        public IEnumerable<TNode> GetOptimalPathBackward()
        {
            ThrowIfNotSuccess();

            var path = new List<TNode>();

            var current = Goal;
            do
            {
                path.Add(current);
                current = cameFrom[current];
            } while (!current.Equals(Start));
            return path;
        }

        public TCost GetOptimalPathCost()
        {
            ThrowIfNotSuccess();
            return bestKnownCostTo[Goal];
        }

        public bool TryGetOptimalPathCost(out TCost? cost)
        {
            if (PathFound)
            {
                cost = bestKnownCostTo[Goal];
                return true;
            }
            cost = default;
            return false;
        }

        private void ThrowIfNotSuccess()
        {
            if (!PathFound)
            {
                throw new Exception("No path found.");
            }
        }
    }

    /// <summary>
    /// For weighted graph analysis, we need to know the other nodes connected to a given node,
    /// and a way to calculate the cost of traversing from that node to its neighbors.
    /// </summary>
    public interface IWeightedGraph<TNode, TCost>
        where TNode : notnull
        where TCost : INumber<TCost>
    {
        /// <summary>
        /// Returns the cost to traverse from node <paramref name="a"/> to node <paramref name="b"/>.
        /// The implementation can assume that this method will only be called on nodes where
        /// <paramref name="b"/> is a neighbor of <paramref name="a"/>, according to the
        /// <see cref="GetNeighbors(TNode)"/> method.
        /// </summary>
        TCost Cost(TNode a, TNode b);

        /// <summary>
        /// Returns the neighbors of the specified node in the graph.
        /// </summary>
        IEnumerable<TNode> GetNeighbors(TNode node);
    }
}
