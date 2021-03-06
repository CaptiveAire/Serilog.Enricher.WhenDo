﻿// Copyright 2016-2018 CaptiveAire Systems
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

using System.Collections.Generic;

using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enricher.WhenDo.Tests
{
    public class LogEventStackSink : ILogEventSink
    {
        public Stack<string> Message { get; } = new Stack<string>();
        public Stack<LogEvent> Events { get; } = new Stack<LogEvent>();
        public void Emit(LogEvent logEvent)
        {
            this.Events.Push(logEvent);
            this.Message.Push(logEvent.RenderMessage());
        }

        public void Clear()
        {
            this.Message.Clear();
            this.Events.Clear();
        }
    }
}