using System;
using System.Collections.Generic;
using System.Linq;

namespace Astronometria.Core.ScientificRun.StateTree
{
    /// <summary>
    /// PURPOSE:
    /// Represents one deterministic ordered path through the M2.4 physics StateTree.
    ///
    /// CONTEXT:
    /// The StateTree is intentionally a directed tree. Knowing the terminal node
    /// fully determines the ordered node path.
    ///
    /// CONSTRAINTS:
    /// The path is immutable and target-independent.
    /// </summary>
    public sealed class PhysicsStateTreePath
    {
        private readonly IReadOnlyList<PhysicsStateNodeType> _nodes;

        public PhysicsStateTreePath(IEnumerable<PhysicsStateNodeType> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            _nodes = nodes.ToList();

            if (_nodes.Count == 0)
                throw new ArgumentException("Physics state tree path must contain at least one node.", nameof(nodes));
        }

        public IReadOnlyList<PhysicsStateNodeType> Nodes => _nodes;

        public PhysicsStateNodeType TerminalNodeType => _nodes[_nodes.Count - 1];

        public bool Contains(PhysicsStateNodeType nodeType)
        {
            if (nodeType == null)
                throw new ArgumentNullException(nameof(nodeType));

            return _nodes.Any(node => node.Equals(nodeType));
        }
    }
}