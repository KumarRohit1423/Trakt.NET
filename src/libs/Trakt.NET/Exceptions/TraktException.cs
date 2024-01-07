﻿namespace TraktNET.Exceptions
{
    /// <summary>Trakt exception.</summary>
    public class TraktException(string message, Exception? innerException = null)
        : Exception(message, innerException)
    {
    }
}
