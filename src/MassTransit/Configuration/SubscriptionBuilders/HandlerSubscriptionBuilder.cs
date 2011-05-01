﻿// Copyright 2007-2011 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.SubscriptionBuilders
{
	using System;
	using Pipeline;
	using SubscriptionConnectors;
	using Subscriptions;

	public class HandlerSubscriptionBuilder<TMessage> :
		SubscriptionBuilder
		where TMessage : class
	{
		readonly HandlerSubscriptionConnector<TMessage> _connector;
		readonly Func<TMessage, Action<TMessage>> _handler;
		readonly Func<UnsubscribeAction, ISubscriptionReference> _referenceFactory;

		public HandlerSubscriptionBuilder(Func<TMessage, Action<TMessage>> handler,
		                                  Func<UnsubscribeAction, ISubscriptionReference> referenceFactory)
		{
			_handler = handler;
			_referenceFactory = referenceFactory;

			_connector = new HandlerSubscriptionConnector<TMessage>();
		}

		public ISubscriptionReference Subscribe(IPipelineConfigurator configurator)
		{
			UnsubscribeAction unsubscribe = _connector.Connect(configurator, _handler);

			return _referenceFactory(unsubscribe);
		}
	}
}