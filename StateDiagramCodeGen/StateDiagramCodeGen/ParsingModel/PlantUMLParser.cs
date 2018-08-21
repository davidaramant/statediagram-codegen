﻿using System.Linq;
using Sprache;

namespace StateDiagramCodeGen.ParsingModel
{
    public static class PlantUMLParser
    {
        static readonly CommentParser Comment = new CommentParser("'", "/'", "'/");

        public static readonly Parser<string> StartDiagram = Parse.String("@startuml").Token().Text();
        public static readonly Parser<string> EndDiagram = Parse.String("@enduml").Token().Text();

        public static readonly Parser<string> StateKeyword = Parse.String("state").Token().Text();

        public static readonly Parser<string> Identifier =
            from leading in Parse.WhiteSpace.Many()
            from first in Parse.Letter.Once()
            from rest in Parse.LetterOrDigit.Many()
            from trailing in Parse.WhiteSpace.Many()
            select new string(first.Concat(rest).ToArray());

        public static readonly Parser<Vertex> SimpleStateDeclaration =
            from state in StateKeyword
            from stateName in Identifier
            select new Vertex(stateName);

        private static readonly Parser<string> Arrow = Parse.String("-->").Token().Text();

        private static readonly Parser<string> Guard =
            from leading in Parse.WhiteSpace.Many()
            from openGuard in Parse.Char('[')
            from guard in Parse.CharExcept(new[] { '[', ']' }).AtLeastOnce().Text()
            from closeGuard in Parse.Char(']')
            from trailing in Parse.WhiteSpace.Many()
            select guard;

        private static readonly Parser<string> Action =
            from leading in Parse.WhiteSpace.Many()
            from openGuard in Parse.Char('/')
            from action in Parse.CharExcept(new[] { '/' }).AtLeastOnce().Text()
            from trailing in Parse.WhiteSpace.Many()
            select action;

        public static readonly Parser<EventTransition> EventTransition =
            from source in Identifier
            from arrow in Arrow
            from destination in Identifier
            from colon in Parse.Char(':')
            from eventName in Identifier
            from guardFunction in Guard.Optional()
            from actionFunction in Action.Optional()
            select new EventTransition(
                source,
                destination,
                eventName,
                actionFunction.GetOrElse(string.Empty),
                guardFunction.GetOrElse(string.Empty));
    }
}
