﻿using System.Net;

namespace TraktNET.Exceptions
{
    public sealed class TraktApiCloudflareException(HttpStatusCode httpStatusCode, HttpMethod httpMethod, HttpRequestMessage requestMessage,
                                                    string? responseContent, Exception? innerException = null)
        : TraktApiException(CreateExceptionMessage(httpStatusCode), httpStatusCode, httpMethod, requestMessage, responseContent, innerException)
    {
    }
}
