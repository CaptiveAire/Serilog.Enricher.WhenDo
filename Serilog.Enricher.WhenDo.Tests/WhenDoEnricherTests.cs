﻿// Copyright 2016 CaptiveAire Systems
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

using FluentAssertions;

using NUnit.Framework;

using Serilog.Events;

namespace Serilog.Enricher.WhenDo.Tests
{
    [TestFixture]
    public class WhenDoEnricherTests
    {
        [Test]
        public void IfLogEventHasPropertyRemovePropertyWorksCorrectly()
        {
            var eventQueueSink = new LogEventStackSink();

            string property = "TestProperty";

            var logger =
                new LoggerConfiguration()
                    .WriteTo.Sink(eventQueueSink, LogEventLevel.Verbose)
                    .Enrich.WithProperty(property, 1)
                    .Enrich.When().HasProperty(property).RemovePropertyIfPresent(property)
                    .CreateLogger();

            logger.Information("Hello");
            var @event = eventQueueSink.Events.Pop();
            @event.Properties.Keys.Should().NotContain(property);
        }
        
        [Test]
        public void IfLogEventHasExceptionRemovePropertyWorksCorrectly()
        {
            var eventQueueSink = new LogEventStackSink();

            string property = "TestProperty";

            var logger =
                new LoggerConfiguration()
                    .WriteTo.Sink(eventQueueSink, LogEventLevel.Verbose)
                    .Enrich.WithProperty(property, 1)
                    .Enrich.When().IsExceptionOf<InvalidCastException>().RemovePropertyIfPresent(property)
                    .CreateLogger();

            // doesn't remove property...
            logger.Information("Hello");
            var @event = eventQueueSink.Events.Pop();
            @event.Properties.Keys.Should().Contain(property);

            // does remove property
            logger.Information(new InvalidCastException("NoGood"), "Hello");
            @event = eventQueueSink.Events.Pop();
            @event.Properties.Keys.Should().NotContain(property);

            // does not match exception, does not remove property
            logger.Information(new ArgumentNullException("NoGood"), "Hello");
            @event = eventQueueSink.Events.Pop();
            @event.Properties.Keys.Should().Contain(property);
        }
    }
}