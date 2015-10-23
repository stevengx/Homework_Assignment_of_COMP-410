using System.Collections.Generic;

namespace Homework3 {
    internal static class Sequence {
        public static IEnumerable<long> Create(long lowerBound, long upperBound) {
            for (var i = lowerBound; i < upperBound; i++) {
                yield return i;
            }
        } 
    }
}