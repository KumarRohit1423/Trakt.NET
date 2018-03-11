﻿namespace TraktApiSharp.Objects.Basic.Json.Writer
{
    using Newtonsoft.Json;
    using Objects.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    internal class NetworkObjectJsonWriter : AObjectJsonWriter<ITraktNetwork>
    {
        public override async Task WriteObjectAsync(JsonTextWriter jsonWriter, ITraktNetwork obj, CancellationToken cancellationToken = default)
        {
            if (jsonWriter == null)
                throw new ArgumentNullException(nameof(jsonWriter));

            await jsonWriter.WriteStartObjectAsync(cancellationToken).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(obj.Network))
            {
                await jsonWriter.WritePropertyNameAsync(JsonProperties.NETWORK_PROPERTY_NAME_NETWORK, cancellationToken).ConfigureAwait(false);
                await jsonWriter.WriteValueAsync(obj.Network, cancellationToken).ConfigureAwait(false);
            }

            await jsonWriter.WriteEndObjectAsync(cancellationToken).ConfigureAwait(false);
        }
    }
}
