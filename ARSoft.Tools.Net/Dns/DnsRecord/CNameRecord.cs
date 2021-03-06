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
	public class CNameRecord : DnsRecordBase
	{
		public string CanonicalName { get; private set; }

		internal CNameRecord() {}

		public CNameRecord(string name, int timeToLive, string canonicalName)
			: base(name, RecordType.CName, RecordClass.INet, timeToLive)
		{
			CanonicalName = canonicalName ?? String.Empty;
		}

		internal override void ParseRecordData(byte[] resultData, int startPosition, int length)
		{
			CanonicalName = DnsMessageBase.ParseDomainName(resultData, ref startPosition);
		}

		internal override string RecordDataToString()
		{
			return CanonicalName;
		}

		protected internal override int MaximumRecordDataLength
		{
			get { return CanonicalName.Length + 2; }
		}

		protected internal override void EncodeRecordData(byte[] messageData, int offset, ref int currentPosition, Dictionary<string, ushort> domainNames)
		{
			DnsMessageBase.EncodeDomainName(messageData, offset, ref currentPosition, CanonicalName, true, domainNames);
		}
	}
}