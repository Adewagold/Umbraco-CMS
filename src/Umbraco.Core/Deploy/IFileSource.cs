﻿using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Umbraco.Core.Deploy
{
    /// <summary>
    /// Represents a file source, ie a mean for a target environment involved in a
    /// deployment to obtain the content of files being deployed.
    /// </summary>
    public interface IFileSource
    {
        /// <summary>
        /// Gets the content of a file as a stream.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <returns>A stream with read access to the file content.</returns>
        /// <remarks>
        /// <para>Returns null if no content could be read.</para>
        /// <para>The caller should ensure that the stream is properly closed/disposed.</para>
        /// </remarks>
        Stream GetFileStream(StringUdi udi);

        /// <summary>
        /// Gets the content of a file as a stream.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A stream with read access to the file content.</returns>
        /// <remarks>
        /// <para>Returns null if no content could be read.</para>
        /// <para>The caller should ensure that the stream is properly closed/disposed.</para>
        /// </remarks>
        Task<Stream> GetFileStreamAsync(StringUdi udi, CancellationToken token);

        /// <summary>
        /// Gets the content of a file as a string.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <returns>A string containing the file content.</returns>
        /// <remarks>Returns null if no content could be read.</remarks>
        string GetFileContent(StringUdi udi);

        /// <summary>
        /// Gets the content of a file as a string.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>A string containing the file content.</returns>
        /// <remarks>Returns null if no content could be read.</remarks>
        Task<string> GetFileContentAsync(StringUdi udi, CancellationToken token);

        /// <summary>
        /// Gets the length of a file.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <returns>The length of the file, or -1 if the file does not exist.</returns>
        long GetFileLength(StringUdi udi);

        /// <summary>
        /// Gets the length of a file.
        /// </summary>
        /// <param name="udi">A file entity identifier.</param>
        /// <param name="token">A cancellation token.</param>
        /// <returns>The length of the file, or -1 if the file does not exist.</returns>
        Task<long> GetFileLengthAsync(StringUdi udi, CancellationToken token);

        // fixme - doc
        void GetFiles(IEnumerable<StringUdi> udis, IFileStore fileStore);

        Task GetFilesAsync(IEnumerable<StringUdi> udis, IFileStore fileStore, CancellationToken token);

        ///// <summary>
        ///// Gets the content of a file as a bytes array.
        ///// </summary>
        ///// <param name="Udi">A file entity identifier.</param>
        ///// <returns>A byte array containing the file content.</returns>
        //byte[] GetFileBytes(StringUdi Udi);
    }
}