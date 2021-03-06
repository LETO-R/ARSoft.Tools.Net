﻿#region Copyright and License
// Copyright 2010..11 Alexander Reinert
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ARSoft.Tools.Net.Dns
{
	public abstract class DnsMessageEntryBase
	{
		public string Name { get; internal set; }
		public RecordType RecordType { get; internal set; }
		public RecordClass RecordClass { get; internal set; }

		internal abstract int MaximumLength { get; }
		internal abstract void Encode(byte[] destination, int offset, ref int currentPosition, Dictionary<string, ushort> domainNames);

		public override string ToString()
		{
			return Name + " " + RecordType + " " + RecordClass;
		}
	}
}