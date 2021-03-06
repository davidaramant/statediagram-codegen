﻿using System.Diagnostics;

namespace StateDiagramCodeGen.MachineModel
{
    [DebuggerDisplay("{_name}")]
    public sealed class ActionReference
    {
        private readonly string _name;

        public ActionReference(string name)
        {
            _name = name;
        }
    }
}
