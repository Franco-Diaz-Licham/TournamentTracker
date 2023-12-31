﻿namespace TrackerUI;

public partial class CreateTeamForm : Form
{
    private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
    private List<PersonModel> selectedTeamMembers = new();
    ITeamRequester callingForm;

    public CreateTeamForm(ITeamRequester caller)
    {
        InitializeComponent();
        // CreateSampleData()
        callingForm = caller;
        WiredUpLists();
    }

    private void WiredUpLists()
    {
        selectTeamMemberDropDown.DataSource = null;
        selectTeamMemberDropDown.DataSource = availableTeamMembers;
        selectTeamMemberDropDown.DisplayMember = "FullName";

        teamMembersListBox.DataSource = null;
        teamMembersListBox.DataSource = selectedTeamMembers;
        teamMembersListBox.DisplayMember = "FullName";
    }

    private void CreateTeamButton_Click(object sender, EventArgs e)
    {
        if (ValidateCreateTeamForm())
        {
            TeamModel team = new(){TeamName = teamNameValue.Text, TeamMembers = selectedTeamMembers};
            GlobalConfig.Connection.CreateTeam(team);
            callingForm.TeamComplete(team);
            this.Close();
        }
    }

    private void CreateMemberButton_Click(object sender, EventArgs e)
    {
        if (ValidateAddPersonForm())
        {
            PersonModel p = new PersonModel();

            p.FirstName = firstNameValue.Text;
            p.LastName = lastNameValue.Text;
            p.EmailAddress = emailLabel.Text;
            p.CellphoneNumber = cellPhoneValue.Text;

            GlobalConfig.Connection.CreatePerson(p);
            selectedTeamMembers.Add(p);
            WiredUpLists();

            // clear the entries of the form
            firstNameValue.Text = "";
            lastNameValue.Text = "";
            emailValue.Text = "";
            cellPhoneValue.Text = "";
        }

        else
        {
            MessageBox.Show("This is not a valid Form, please check and try again.");
        }
    }

    private bool ValidateAddPersonForm()
    {
        // define variables
        bool output = true;
        string firstName = firstNameValue.Text;
        string lastName = lastNameValue.Text;
        string email = emailValue.Text;
        string phoneNumber = cellPhoneValue.Text;

        // check validation conditions
        if (firstName.Length == 0)
        {
            output = false;
        }
        if (lastName.Length == 0)
        {
            output = false;
        }
        if (email.Length == 0 &&
            email.Contains("@"))
        {
            output = false;
        }
        if (phoneNumber.Length == 0)
        {
            output = false;
        }
        return output;
    }

    private bool ValidateCreateTeamForm() 
    {
        bool output = true;
        string teamName = teamNameValue.Text;

        if (teamName.Length == 0)
        {
            output = false;
        }
        return output;

    }

    private void AddMemberButton_Click(object sender, EventArgs e)
    {
        if (selectTeamMemberDropDown.SelectedItem != null)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;
            availableTeamMembers.Remove(p);
            selectedTeamMembers.Add(p);
            WiredUpLists();
        }
    }

    private void RemoveSelectedMemberButton_Click(object sender, EventArgs e)
    {
        if (teamMembersListBox.SelectedItem != null)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            selectedTeamMembers.Remove(p);
            availableTeamMembers.Add(p);
            WiredUpLists();
        }
    }
}
