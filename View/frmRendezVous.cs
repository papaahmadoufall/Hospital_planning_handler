using System;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using WindowsFormsApp1.Model;

namespace WindowsFormsApp1.View
{
    public partial class frmRendezVous : Form
    {
        BdRvMedicalContext bd = new BdRvMedicalContext();

        public frmRendezVous()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.ControlBox = false;  // Supprimer les boutons de contrôle
            this.ShowIcon = false;    // Supprimer l'icône
            this.ShowInTaskbar = false; // Ne pas afficher dans la barre des tâches
        }

        private void frmRendezVous_Load(object sender, EventArgs e)
        {
            ResetForm();
            LoadPhoneNumbers();
            LoadBloodGroups();  // Charger les groupes sanguins
        }

        private void LoadPhoneNumbers()
        {
            var phoneDetails = bd.Patients.Include(p => p.GroupeSanguin).Select(p => new
            {
                p.TEL,
                p.NomPrenom,
            }).ToList();

            AutoCompleteStringCollection phoneCollection = new AutoCompleteStringCollection();

            foreach (var detail in phoneDetails)
            {
                string formattedEntry = $"{detail.TEL} - {detail.NomPrenom}";
                phoneCollection.Add(formattedEntry);
            }

            cbbTelephone.AutoCompleteCustomSource = phoneCollection;
            cbbTelephone.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbbTelephone.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void LoadBloodGroups()
        {
            // Récupérer tous les groupes sanguins depuis la base de données
            var bloodGroups = bd.GroupeSanguins.ToList();

            // Ajouter chaque groupe sanguin au ComboBox
            cbbGroupeSanguin.Items.Clear();  // Effacer les anciens items, si nécessaires
            foreach (var group in bloodGroups)
            {
                cbbGroupeSanguin.Items.Add(group.CodeGroupeSanguin); // Ajoute chaque groupe sanguin
            }
        }

        private void ResetForm()
        {
            txtNomPrenom.Clear();
            txtAdresse.Clear();
            txtEmail.Clear();
            txtPoids.Clear();
            txtTaille.Clear();
            cbbGroupeSanguin.SelectedIndex = -1;
            cbbTelephone.SelectedIndex = -1;
        }

        private void btnRenitialiser_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            // Validation ou enregistrement des données si nécessaire
            MessageBox.Show("Données validées.");
        }

        private void cbbTelephone_TextChanged(object sender, EventArgs e)
        {
            // Si le texte dans cbbTelephone change, on vérifie si le numéro correspond à un patient
            var phoneParts = cbbTelephone.Text.Split(new string[] { " - " }, StringSplitOptions.None);
            string phoneNumber = phoneParts[0].Trim(); // On prend uniquement le numéro

            // Charger les données du patient correspondant au numéro de téléphone
            UpdatePatientDetails(phoneNumber);
        }

        private void cbbTelephone_Leave(object sender, EventArgs e)
        {
            // Lorsqu'on quitte le champ, on vérifie le numéro saisi
            var phoneParts = cbbTelephone.Text.Split(new string[] { " - " }, StringSplitOptions.None);
            if (phoneParts.Length > 0)
            {
                string phoneNumber = phoneParts[0].Trim(); // On garde uniquement le numéro de téléphone
                cbbTelephone.Text = phoneNumber; // Remet le numéro dans le champ
            }
            else
            {
                cbbTelephone.Text = string.Empty; // Si le champ est vide, on efface tout
            }
        }

        private void cbbTelephone_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lorsque l'utilisateur sélectionne une valeur dans la liste d'autocomplétion
            var selectedText = cbbTelephone.SelectedItem.ToString();
            var phoneParts = selectedText.Split(new string[] { " - " }, StringSplitOptions.None);

            if (phoneParts.Length > 0)
            {
                string phoneNumber = phoneParts[0].Trim(); // Garder uniquement le numéro de téléphone
                cbbTelephone.Text = phoneNumber; // Mettre à jour le champ avec le numéro

                // Recherche du patient correspondant au numéro de téléphone
                var patient = bd.Patients.Include(p => p.GroupeSanguin).FirstOrDefault(p => p.TEL == phoneNumber);

                if (patient != null)
                {
                    // Si un patient est trouvé, remplir les champs
                    txtNomPrenom.Text = patient.NomPrenom ?? string.Empty;
                    txtAdresse.Text = patient.Adresse ?? string.Empty;
                    txtEmail.Text = patient.Email ?? string.Empty;
                    txtPoids.Text = patient.Poids?.ToString() ?? string.Empty;
                    txtTaille.Text = patient.Taille?.ToString() ?? string.Empty;

                    // Remplir le groupe sanguin
                    if (patient.GroupeSanguin != null)
                    {
                        // Sélectionner le groupe sanguin dans le ComboBox
                        cbbGroupeSanguin.SelectedItem = patient.GroupeSanguin.CodeGroupeSanguin ?? string.Empty;
                    }
                }
                else
                {
                    // Si aucun patient trouvé, réinitialiser les champs
                    ResetForm();
                }
            }
        }

        private void UpdatePatientDetails(string phoneNumberInput)
        {
            // Vérifie si le texte dans le champ cbbTelephone est valide
            if (!string.IsNullOrEmpty(phoneNumberInput))
            {
                // Recherche du patient en utilisant seulement le numéro de téléphone (avant le premier " - ")
                var phoneParts = phoneNumberInput.Split(new string[] { " - " }, StringSplitOptions.None);
                if (phoneParts.Length > 0)
                {
                    string phoneNumber = phoneParts[0].Trim(); // On prend uniquement le numéro

                    var patient = bd.Patients.Include(p => p.GroupeSanguin).FirstOrDefault(p => p.TEL == phoneNumber);

                    if (patient != null)
                    {
                        // Si un patient est trouvé, on remplit les champs du formulaire avec les données du patient
                        txtNomPrenom.Text = patient.NomPrenom ?? string.Empty;
                        txtAdresse.Text = patient.Adresse ?? string.Empty;
                        txtEmail.Text = patient.Email ?? string.Empty;
                        txtPoids.Text = patient.Poids?.ToString() ?? string.Empty;
                        txtTaille.Text = patient.Taille?.ToString() ?? string.Empty;

                        // Remplir le groupe sanguin dans le combo box
                        if (patient.GroupeSanguin != null)
                        {
                            cbbGroupeSanguin.SelectedItem = patient.GroupeSanguin.CodeGroupeSanguin ?? string.Empty;
                        }
                        DisableFields(patient);

                    }
                    else
                    {
                        // Si aucun patient n'est trouvé, on vide les champs
                        ResetForm();
                        EnableAllFields();
                    }
                }
            }
            else
            {
                // Si le champ est vide, on vide également les champs
                ResetForm();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void cbbGroupeSanguin_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Vous pouvez ajouter des actions supplémentaires ici si nécessaire lorsque le groupe sanguin est sélectionné
        }


        private void DisableFields(Patient patient)
        {
            // Activer les champs si la valeur est null ou vide, sinon désactiver les champs
            txtNomPrenom.Enabled = string.IsNullOrEmpty(patient.NomPrenom);
            txtAdresse.Enabled = string.IsNullOrEmpty(patient.Adresse);
            txtEmail.Enabled = string.IsNullOrEmpty(patient.Email);
            txtPoids.Enabled = patient.Poids == null || patient.Poids == 0; // Peut aussi vérifier si Poids est égal à 0 (si c'est le cas)
            txtTaille.Enabled = patient.Taille == null || patient.Taille == 0; // Même logique pour la taille
            cbbGroupeSanguin.Enabled = string.IsNullOrEmpty(patient.GroupeSanguin?.CodeGroupeSanguin); // Vérifie si le groupe sanguin est null ou vide
        }


        private void EnableAllFields()
        {
            // Activer tous les champs de saisie
            txtNomPrenom.Enabled = true;
            txtAdresse.Enabled = true;
            txtEmail.Enabled = true;
            txtPoids.Enabled = true;
            txtTaille.Enabled = true;
            cbbGroupeSanguin.Enabled = true;  // Activer le ComboBox du groupe sanguin
        }
    }
}
