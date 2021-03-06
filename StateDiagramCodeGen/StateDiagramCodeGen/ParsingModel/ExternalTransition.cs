﻿namespace StateDiagramCodeGen.ParsingModel
{
    public sealed class ExternalTransition : IDiagramElement
    {
        public string Source { get; }
        public string Destination { get; }
        public string EventName { get; }
        public string GuardName { get; }
        public string ActionName { get; }

        public bool IsInitialTransition => Source == Constants.Star;

        public ExternalTransition(
            string source, 
            string destination, 
            string eventName, 
            string guardName, 
            string actionName)
        {
            Source = source;
            Destination = destination;
            EventName = eventName;
            GuardName = guardName;
            ActionName = actionName;
        }
    }
}
