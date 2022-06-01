using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace tFramework.Interfaces
{
    public interface IThread
    {
        /// <summary>
        /// Method that called before run method, to initalize all are need
        /// </summary>
        void Start();

        /// <summary>
        /// Method that contains the thread method.
        /// </summary>
        /// <returns>True if thread continue, or false to stop thread loop!</returns>
        bool Run();

        /// <summary>
        /// Method called on the thread stopped or killed
        /// </summary>
        void End();
    }
}
