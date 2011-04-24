using Microsoft.Practices.Unity;

namespace CDTag.Common
{
    /// <summary>
    /// Unity static class.
    /// </summary>
    public static class Unity
    {
        /// <summary>Gets or sets the Unity container.</summary>
        /// <value>The Unity container.</value>
        public static IUnityContainer Container { get; set; }

        /// <summary>Resolves the specified type.</summary>
        /// <typeparam name="T">Type type.</typeparam>
        /// <returns>An instance of the type.</returns>
        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
