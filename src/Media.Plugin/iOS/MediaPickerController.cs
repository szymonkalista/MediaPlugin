﻿using System;
using System.Threading.Tasks;

using Plugin.Media.Abstractions;

using UIKit;
using Foundation;
using System.Collections.Generic;

namespace Plugin.Media
{
    /// <summary>
    /// Media Picker Controller
    /// </summary>
    public sealed class MediaPickerController : UIImagePickerController
    {
        TaskCompletionSource<Task> taskCompletionSource;

        internal MediaPickerController(MediaPickerDelegate mpDelegate, TaskCompletionSource<Task> taskCompletionSource)
        { 
			base.Delegate = mpDelegate;
            this.taskCompletionSource = taskCompletionSource;
	    }
        

        /// <summary>
        /// Deleage
        /// </summary>
        public override NSObject Delegate
        {
            get => base.Delegate; 
            set
            {   
                if (value == null)
                    base.Delegate = value;
                else
                    throw new NotSupportedException();
            }
        }

        public override void ViewDidDisappear(bool animated)
        { 
	        base.ViewDidDisappear(animated);
            this.taskCompletionSource.TrySetResult(null);
	    }

        /// <summary>
        /// Gets result of picker
        /// </summary>
        /// <returns></returns>

        public Task<List<MediaFile>> GetResultAsync() =>
            ((MediaPickerDelegate)Delegate).Task;

        bool disposed;
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if(disposing && !disposed)
            {
                disposed = true;
                InvokeOnMainThread(() =>
                {
					try
					{
						Delegate?.Dispose();
						Delegate = null;
					}
					catch
					{

					}
                });
            }
        }
    }
}