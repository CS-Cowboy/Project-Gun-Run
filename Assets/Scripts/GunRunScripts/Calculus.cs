using System;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    /// <summary>
    /// A class to encapsulate and calculate the position, first, and second derivatives of any linear, two, or three dimensional value. 
    /// </summary>
    public class Calculus : MonoBehaviour
    {
        protected float linearPos, linearPrev, firstLinearDerivation, prevFirst_derivation, secondLinearDerivation;
        protected Vector2 two_spacePosition, two_spacePrev, two_spaceDerivation, two_spacePrevFirst_derivation, two_spaceSecondDerivation;
        protected Vector3 three_spacePosition, three_spacePrev, three_spaceDerivation, three_spacePrevFirst_derivation, three_spaceSecondDerivation;
        /// <summary>
        /// The given value.
        /// </summary>
        /// <value> encapsulated and unmodified value of input.</value>
        public float Position
        {
            get
            {
                return linearPos;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The first derivative (velocity). </value>
        public float Velocity
        {
            get
            {
                return firstLinearDerivation;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The second derivative (acceleration). </value>
        public float Acceleration
        {
            get
            {
                return secondLinearDerivation;
            }
        }

        /// <summary>
        /// The given value.
        /// </summary>
        /// <value> encapsulated and unmodified value of input.</value>
        public Vector2 TwoSpacePosition
        {
            get
            {
                return two_spacePosition;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The first derivative (velocity). </value>
        public Vector2 TwoSpaceVelocity
        {
            get
            {
                return two_spaceDerivation;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The second derivative (acceleration). </value>
        public Vector2 TwoSpaceAcceleration
        {
            get
            {
                return two_spaceSecondDerivation;
            }
        }


        /// <summary>
        /// The given value.
        /// </summary>
        /// <value> encapsulated and unmodified value of input.</value>
        public Vector3
        ThreeSpacePosition
        {
            get
            {
                return three_spacePosition;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The first derivative (velocity). </value>
        public Vector3 ThreeSpaceVelocity
        {
            get
            {
                return three_spaceDerivation;
            }
        }
        /// <summary>
        /// </summary>
        /// <value>The second derivative (acceleration). </value>
        public Vector3 ThreeSpaceAcceleration
        {
            get
            {
                return three_spaceSecondDerivation;
            }
        }


        /// <summary>
        /// Computesfloathe derivative offloathe value given infloathe object.   
        /// </summary>
        /// <param name="b">The current value.</param>
        /// <param name="a">The old value.</param>
        /// <returns>The differentiated value.</returns>
        protected Vector3 Differentiate(Vector3 a, ref Vector3 b)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        protected Vector2 Differentiate(Vector2 a, ref Vector2 b)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        protected float Differentiate(float a, ref float b)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        /// <summary>
        /// Computesfloathe derivative and setsfloathe input for first and second differentiable T's. 
        /// </summary>
        /// <param name="input"></param>
        public void Compute(Vector3 input)
        {
            three_spacePosition = input;
            three_spaceDerivation = Differentiate(three_spacePosition, ref three_spacePrev);
            three_spaceSecondDerivation = Differentiate(three_spaceDerivation, ref three_spacePrevFirst_derivation);
        }
        public void Compute(Vector2 input)
        {
            two_spacePosition = input;
            two_spaceDerivation = Differentiate(two_spacePosition, ref two_spacePrev);
            two_spaceSecondDerivation = Differentiate(two_spaceDerivation, ref two_spacePrevFirst_derivation);
        }
        public void Compute(float input)
        {
            linearPos = input;
            firstLinearDerivation = Differentiate(linearPos, ref linearPrev);
            secondLinearDerivation = Differentiate(firstLinearDerivation, ref prevFirst_derivation);
        }


    }
}
