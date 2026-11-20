using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Extensions
{
    public static class ListExtensions
    {

        public static T SelectClosest<T>(
            this IEnumerable<T> values,
            Vector3 sourcePosition,
            params Func<T, bool>[] conditions)
            where T : IHasPosition
        {
            return SelectClosest(
                values,
                sourcePosition,
                out _,
                out _,
                conditions);
        }

        public static T SelectClosest<T>(
            this IEnumerable<T> values,
            Vector3 sourcePosition,
            out Vector3 position,
            out float distance,
            params Func<T, bool>[] conditions)
            where T : IHasPosition
        {
            T closest = default;
            position = default;
            distance = float.MaxValue;

            foreach (var value in values)
            {
                var pass = true;

                foreach (var condition in conditions)
                {
                    if (!condition(value))
                    {
                        pass = false;
                        continue;
                    }
                }

                if (!pass)
                    continue;

                var currentPosition = value.Position;
                var axis = sourcePosition - currentPosition;
                var currentDistance = Vector3.SqrMagnitude(axis);

                if (distance < currentDistance)
                    continue;

                position = currentPosition;
                distance = currentDistance;
                closest = value;
            }

            distance = Mathf.Sqrt(distance);

            return closest;
        }
    }
}
