package md53e70eefec93f68da7a60ae5ce7e8bc2b;


public class FirebaseIIDService
	extends com.google.firebase.iid.FirebaseInstanceIdService
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onTokenRefresh:()V:GetOnTokenRefreshHandler\n" +
			"";
		mono.android.Runtime.register ("SaleItemMobile.PushHelper.FirebaseIIDService, SaleItemMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", FirebaseIIDService.class, __md_methods);
	}


	public FirebaseIIDService () throws java.lang.Throwable
	{
		super ();
		if (getClass () == FirebaseIIDService.class)
			mono.android.TypeManager.Activate ("SaleItemMobile.PushHelper.FirebaseIIDService, SaleItemMobile, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onTokenRefresh ()
	{
		n_onTokenRefresh ();
	}

	private native void n_onTokenRefresh ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
