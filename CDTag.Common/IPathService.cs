namespace CDTag.Common
{
    /// <summary>
    /// IPathService
    /// </summary>
    public interface IPathService
    {
        /// <summary>Gets the local application directory.</summary>
        string LocalApplicationDirectory { get; }

        /// <summary>Gets the profile directory.</summary>
        string ProfileDirectory { get; }

        /// <summary>Determines whether a short file name is valid.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        ///   <c>true</c> if the short file name is valid; otherwise, <c>false</c>.
        /// </returns>
        bool IsShortFileNameValid(string fileName);

        /// <summary>Gets the English character approximation of a string containing characters from other languages.</summary>
        /// <param name="input">The input.</param>
        /// <returns>The English character approximation.</returns>
        string GetEnglishCharacterReplacement(string input);
    }
}
