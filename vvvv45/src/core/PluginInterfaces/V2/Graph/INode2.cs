﻿using System;
using VVVV.Core;

namespace VVVV.PluginInterfaces.V2.Graph
{
    public interface INode2 : IViewableList<INode2>, INamed
    {
        /// <summary>
        /// Reference to the internal COM interface. Use with caution.
        /// </summary>
        INode InternalCOMInterf
        {
            get;
        }
        
        INodeInfo NodeInfo
        {
            get;
        }
        
        int ID
        {
            get;
        }
        
        IViewableCollection<IPin2> Pins
        {
            get;
        }
        
        /// <summary>
		/// Gets the <see cref="IWindow2">window</see> of this node. Or null if
		/// this node doesn't have a window.
		/// </summary>
		IWindow2 Window
		{
			get;
		}
        
        /// <summary>
		/// Gets or sets the last runtime error that occured or null if there were no errors.
		/// </summary>
		string LastRuntimeError
		{
			get;
			set;
		}
		
		/// <summary>
		/// Provides access to the parent node.
		/// </summary>
		INode2 Parent
		{
			get;
		}
		
		bool HasPatch
		{
			get;
		}
		
		bool HasCode
		{
			get;
		}
		
		bool HasGUI
		{
			get;
		}
		
		StatusCode Status
		{
			get;
		}
		
		StatusCode InnerStatus
		{
			get;
		}
		
		event EventHandler StatusChanged;
		
		event EventHandler InnerStatusChanged;
    }
	
	public static class Node2ExtensionMethods
	{
		public static bool ContainsMissingNodes(this INode2 node)
		{
			return (node.InnerStatus & StatusCode.IsMissing) == StatusCode.IsMissing;
		}
		
		public static bool ContainsBoygroupedNodes(this INode2 node)
		{
			return (node.InnerStatus & StatusCode.IsBoygrouped) == StatusCode.IsBoygrouped;
		}
		
		public static bool IsMissing(this INode2 node)
		{
			return (node.Status & StatusCode.IsMissing) == StatusCode.IsMissing;
		}
		
		public static bool IsBoygrouped(this INode2 node)
		{
			return (node.Status & StatusCode.IsBoygrouped) == StatusCode.IsBoygrouped;
		}
	}
}