using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PrivateMethodAccess;

public partial class FormSecureService: Form
{
    public delegate string AuthorizedAction();

    public FormSecureService()
    {
        InitializeComponent();
    }

    // Private method we want to protect
    private string PerformSecureAction()
    {
        return "Sensitive operation executed with success.";
    }

    public AuthorizedAction GetAuthorizedDelegate(string callerId)
    {
        if (IsAuthorized(callerId))
        {
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
            var authorizedAction = GetAuthorizedDelegate(callerId);
            textBox1.Text = authorizedAction();

        }
        catch (Exception ex)
        {
            textBox1.Text = ex.Message;
        }
    }
}
