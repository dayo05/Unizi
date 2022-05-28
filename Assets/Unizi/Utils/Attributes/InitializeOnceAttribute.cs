using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Unizi.Utils.Attributes
{
    [AttributeUsage(AttributeTargets.Constructor)]
    public class InitializeOnceAttribute: Attribute
    {
        private static readonly HashSet<string> Initialized = new();
        public InitializeOnceAttribute([CallerMemberName] [CanBeNull] string dat = null)
        {
            if (dat == null) throw new ArgumentNullException(nameof(dat));
            if (Initialized.Contains(dat))
                throw new InvalidOperationException($"{dat} initialized twice.");
            Initialized.Add(dat);
        }

        internal static void Remove([CanBeNull] string dat)
        {
            if (dat == null) throw new ArgumentNullException(nameof(dat));
            if(!Initialized.Contains(dat))
                throw new InvalidOperationException($"{dat} not initialized.");
            Initialized.Remove(dat);
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class DestroyInitializationStateAttribute : Attribute
    {
        public DestroyInitializationStateAttribute([CallerMemberName] [CanBeNull] string dat = null)
            => InitializeOnceAttribute.Remove(dat);
    }
}