using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrivateMethodAccess;

public partial class FormSecureService: Form
{
    public delegate void AuthorizedAction();

    public FormSecureService()
    {
        InitializeComponent();
    }

    // Private method we want to protect
    private void PerformSecureAction()
    {
        textBox1.Text = $"Sensitive operation executed with success.";
    }        

    // Public method that returns a delegate only to authorized callers
    public AuthorizedAction GetAuthorizedDelegate(string callerId)
    {
        if (IsAuthorized(callerId))
        {
            // Return a delegate pointing to the private method
            return PerformSecureAction;
        }
        else
        {
            throw new UnauthorizedAccessException($"Access denied for caller: {callerId}");
        }  
    }

    private bool IsAuthorized(string callerId)
    {
        // Simulated authorization logic
        var authorizedCallers = new HashSet<string> { "Admin", "TrustedService" };
        return authorizedCallers.Contains(callerId);
    }

    private void button1_Click(object sender, EventArgs e)
    {
        textBox1.Text = string.Empty;

        try
        {
            var callerId = comboBox1.SelectedItem?.ToString() ?? string.Empty;
            var AuthorizedAction = GetAuthorizedDelegate(callerId);
            AuthorizedAction();
        }
        catch (Exception ex)
        {

            textBox1.Text = ex.Message;
        }
    }
}
