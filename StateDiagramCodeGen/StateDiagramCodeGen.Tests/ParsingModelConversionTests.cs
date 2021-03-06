﻿using NUnit.Framework;
using Sprache;
using StateDiagramCodeGen.ParsingModel;

namespace StateDiagramCodeGen.Tests
{
    [TestFixture]
    public sealed class ParsingModelConversionTests
    {
        [Test]
        public void ShouldConvertStates()
        {
            const string input = @"@startuml ""Simple Diagram""
                state Off
                state On {
                    state Idle
                    state Responding
                }
                @enduml";

            var diagram = PlantUmlParser.Diagram.End().Parse(input);

            Assert.DoesNotThrow(() => { var machine = diagram.ToMachineModel(); });
        }
    }
}
