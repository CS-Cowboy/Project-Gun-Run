using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.braineeeeDevs.gunRun
{
    /// <summary>
    /// A class to encapsulate and calculate the position, first, and second derivatives of any Vector3. 
    /// </summary>
    public class Calculus : MonoBehaviour
    {
        protected Vector3 oldPosition, oldFirstDerivative, firstDerivative, secondDerivative, positionalValue;
        /// <summary>
        /// The given value.
        /// </summary>
        /// <value>The encapsulated and unmodified value of input.</value>
        public Vector3 Position
        {
            get
            {
                return positionalValue;
            }
    }
    /// <summary>
    /// The first derivative of the encapsulated value.
    /// </summary>
    /// <value>The first derivative (velocity). </value>
        public Vector3 Velocity
        {
            get
            {
                return firstDerivative;
            }
        }
        /// <summary>
        /// The second derivative of the encapsulated value.
        /// </summary>
        /// <value>The second derivative (acceleration). </value>
        public Vector3 Acceleration
        {
            get
            {
                return secondDerivative;
            }
        }

        /// <summary>
        /// Computes the derivative of the value given in Vector3 b.
        /// </summary>
        /// <param name="b">The current value.</param>
        /// <param name="a">The old value.</param>
        /// <returns>The differentiated value.</returns>
        protected Vector3 Differentiate(Vector3 b, ref Vector3 a)
        {
            Vector3 derivative = b - a;
            b = a;
            return derivative;  
        }
        /// <summary>
        /// Computes the derivative and sets the input for first and second differentiable Vector3's. 
        /// </summary>
        /// <param name="input"></param>
        public void Compute(Vector3 input)
        {
            positionalValue = input;
            firstDerivative = Differentiate(positionalValue, ref oldPosition);
            secondDerivative = Differentiate(firstDerivative, ref oldFirstDerivative);
        }
        
    }
}
