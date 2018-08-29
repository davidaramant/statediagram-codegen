﻿using System;
using Humanizer;
using Sprache;
using StateDiagramCodeGen.PlantUMLModel;

namespace StateDiagramCodeGen.Model
{
    public static class PlantUmlParser
    {
        public static readonly Parser<string> Identifier =
            from leading in Parse.WhiteSpace.Many()
            from id in Parse.Identifier(Parse.Letter, Parse.LetterOrDigit).Token()
            from trailing in Parse.WhiteSpace.Many()
            select id;

        public static readonly Parser<Vertex> SimpleStateDeclaration =
            from leading in Parse.WhiteSpace.Many()
            from state in Parse.String("state")
            from stateName in Identifier
            select new Vertex(stateName);

        public static readonly Parser<string> DehumanizedSentence =
            from sentence in Parse.LetterOrDigit.Or(Parse.Char(' ')).Many().Text().Token()
            select sentence.Dehumanize();

        public static readonly Parser<string> MethodReference =
            from id in Identifier
            from parens in Parse.String("()").Optional()
            select id;

        public static readonly Parser<string> FriendlyMethodReference = DehumanizedSentence.Or(MethodReference);

        private static readonly Parser<string> Arrow = Parse.String("-->").Token().Text();

        private static readonly Parser<string> Guard =
            from leading in Parse.WhiteSpace.Many()
            from openGuard in Parse.Char('[')
            from guard in FriendlyMethodReference
            from closeGuard in Parse.Char(']')
            from trailing in Parse.WhiteSpace.Many()
            select guard;

        private static readonly Parser<string> Action =
            from leading in Parse.WhiteSpace.Many()
            from openGuard in Parse.Char('/')
            from action in FriendlyMethodReference
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
                source: source,
                destination: destination,
                eventName : eventName,
                guardName: guardFunction.GetOrElse(string.Empty),
                actionName: actionFunction.GetOrElse(string.Empty));

        public static readonly Parser<EntryAction> EntryAction =
            from leading in Parse.WhiteSpace.Many()
            from stateName in Identifier
            from colon in Parse.Char(':')
            from ws in Parse.WhiteSpace.Many()
            from eventName in Parse.String("entry")
            from guardFunction in Guard.Optional()
            from actionFunction in Action.Optional()
            select new EntryAction(
                stateName:stateName,
                guardName:guardFunction.GetOrElse(string.Empty), 
                actionName:actionFunction.GetOrElse(string.Empty));

        public static readonly Parser<ExitAction> ExitAction =
            from leading in Parse.WhiteSpace.Many()
            from stateName in Identifier
            from colon in Parse.Char(':')
            from ws in Parse.WhiteSpace.Many()
            from eventName in Parse.String("exit")
            from guardFunction in Guard.Optional()
            from actionFunction in Action.Optional()
            select new ExitAction(
                stateName:stateName,
                guardName:guardFunction.GetOrElse(string.Empty), 
                actionName:actionFunction.GetOrElse(string.Empty));

        public static readonly Parser<InternalTransition> InternalTransition =
            from leading in Parse.WhiteSpace.Many()
            from stateName in Identifier
            from colon in Parse.Char(':')
            from eventName in Identifier
            from guardFunction in Guard.Optional()
            from actionFunction in Action.Optional()
            select new InternalTransition(
                stateName:stateName,
                eventName:eventName, 
                guardName:guardFunction.GetOrElse(string.Empty), 
                actionName:actionFunction.GetOrElse(string.Empty));


        static readonly CommentParser Comment = new CommentParser("'", "/'", "'/");

        public static readonly Parser<string> StartDiagram = Parse.String("@startuml").Token().Text();
        public static readonly Parser<string> EndDiagram = Parse.String("@enduml").Token().Text();
    }
}
