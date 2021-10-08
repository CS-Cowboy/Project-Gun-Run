using System;
using UnityEngine;

namespace com.braineeeeDevs.gr
{
    /// <summary>
    /// A class to encapsulate and calculate the position, first, and second derivatives of any linear, two, or three dimensional value. 
    /// </summary>
    public class Calculus : MonoBehaviour
    {
        protected float linearPos, firstLinearDerivation, prevLinearFirst_derivation, secondLinearDerivation, prevSecondLinearDerivation;
        protected Vector2 two_spacePosition, two_spaceFirstDerivation, prevTwo_spaceFirst_derivation, two_spaceSecond_derivation, prevTwo_spaceSecond_derivation;
        protected Vector3 three_spacePosition, three_spaceFirstDerivation, prevthree_spaceFirst_derivation, three_spaceSecond_derivation, prevThree_spaceSecond_derivation;


        /// <summary>
        /// The given float value.
        /// </summary>
        /// <value> The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public float Position
        {
            get
            {
                return linearPos;
            }
        }
        /// <summary>
        /// The float first derivative (velocity). 
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public float Velocity
        {
            get
            {
                return firstLinearDerivation;
            }
        }
        /// <summary>
        /// The float second derivative (acceleration).
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute(). </value>
        public float Acceleration
        {
            get
            {
                return secondLinearDerivation;
            }
        }


        /// <summary>
        /// The given Vector3 value.
        /// </summary>
        /// <value> The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public Vector2 TwoSpacePosition
        {
            get
            {
                return two_spacePosition;
            }
        }
        /// <summary>
        /// The Vector3 first derivative (velocity). 
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public Vector2 TwoSpaceVelocity
        {
            get
            {
                return two_spaceFirstDerivation;
            }
        }
        /// <summary>
        /// The Vector3 second derivative (acceleration).
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute(). </value>
        public Vector2 TwoSpaceAcceleration
        {
            get
            {
                return two_spaceSecond_derivation;
            }
        }

        /// <summary>
        /// The given Vector3 value.
        /// </summary>
        /// <value> The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public Vector3 ThreeSpacePosition
        {
            get
            {
                return three_spacePosition;
            }
        }
        /// <summary>
        /// The Vector3 first derivative (velocity). 
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute().</value>
        public Vector3 ThreeSpaceVelocity
        {
            get
            {
                return three_spaceFirstDerivation;
            }
        }
        /// <summary>
        /// The Vector3 second derivative (acceleration).
        /// </summary>
        /// <value>The encapsulated and unmodified value of input to Calculus.Compute(). </value>
        public Vector3 ThreeSpaceAcceleration
        {
            get
            {
                return three_spaceSecond_derivation;
            }
        }


        /// <summary>
        /// Computes the derivative of the Vector3 of the given object.   
        /// </summary>
        /// <param name="b">The current value.</param>
        /// <param name="a">The old value.</param>
        /// <returns>The differentiated value.</returns>
        protected Vector3 Differentiate(Vector3 b, ref Vector3 a)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        /// <summary>
        /// Computes the derivative of the Vector2 of the given object.   
        /// </summary>
        /// <param name="b">The current value.</param>
        /// <param name="a">The old value.</param>
        /// <returns>The differentiated value.</returns>        
        protected Vector2 Differentiate(Vector2 b, ref Vector2 a)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        /// <summary>
        /// Computes the derivative of the float of the given object.   .   
        /// </summary>
        /// <param name="b">The current value.</param>
        /// <param name="a">The old value.</param>
        /// <returns>The differentiated value.</returns>
        protected float Differentiate(float b, ref float a)
        {
            var diff = b - a;
            a = b;
            return diff;
        }

        /// <summary>
        /// Computes the derivative and sets the input for first and second differentiable Vector3's. 
        /// </summary>  
        /// <param name="input">A Vector3 containing the positional value to differentiate.</param>
        public void Compute(Vector3 input)
        {
            three_spacePosition = input;
            three_spaceFirstDerivation = Differentiate(three_spacePosition, ref prevthree_spaceFirst_derivation);
            three_spaceSecond_derivation = Differentiate(three_spaceFirstDerivation, ref prevThree_spaceSecond_derivation);
        }
        /// <summary>
        /// Computes the derivative and sets the input for first and second differentiable Vector2's. 
        /// </summary>  
        /// <param name="input">A Vector2 containing the positional value to differentiate.</param>
        public void Compute(Vector2 input)
        {
            two_spacePosition = input;
            two_spaceFirstDerivation = Differentiate(two_spacePosition, ref prevTwo_spaceFirst_derivation);
            two_spaceSecond_derivation = Differentiate(two_spaceFirstDerivation, ref prevTwo_spaceSecond_derivation);
        }      /// <summary>
               /// Computes the derivative and sets the input for first and second differentiable floats.
               /// </summary>  
               /// <param name="input">A float containing the positional value to differentiate.</param>
               /// <param name="circularRange">Whether to adjust the diffentiation process to accomodate for repeating ranges (such as angular position).</param>
        public void Compute(float input)
        {
            linearPos = input;
            firstLinearDerivation = Differentiate(linearPos, ref prevLinearFirst_derivation);
            secondLinearDerivation = Differentiate(firstLinearDerivation, ref prevSecondLinearDerivation);
        }


    }
}
