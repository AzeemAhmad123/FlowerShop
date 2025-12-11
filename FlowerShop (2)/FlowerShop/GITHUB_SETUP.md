# 📤 Pushing FlowerShop to GitHub

Follow these steps to push your FlowerShop project to GitHub.

## Step 1: Create a GitHub Repository

1. Go to [GitHub](https://github.com) and sign in
2. Click the "+" icon in the top right corner
3. Select "New repository"
4. Fill in the details:
   - **Repository name**: `FlowerShop` (or your preferred name)
   - **Description**: "E-commerce web application for a flower shop built with ASP.NET Core MVC"
   - **Visibility**: Choose Public or Private
   - **DO NOT** initialize with README, .gitignore, or license (we already have these)
5. Click "Create repository"

## Step 2: Initialize Git in Your Project

Open a terminal/command prompt in the `FlowerShop` folder (where the .sln file is located) and run:

```bash
git init
```

## Step 3: Add Files to Git

Add all files to the staging area:

```bash
git add .
```

## Step 4: Create Initial Commit

Commit the files:

```bash
git commit -m "Initial commit: FlowerShop e-commerce application"
```

## Step 5: Connect to GitHub Repository

Replace `<your-username>` with your GitHub username and `<repository-name>` with your repo name:

```bash
git remote add origin https://github.com/<your-username>/<repository-name>.git
```

Example:
```bash
git remote add origin https://github.com/johndoe/FlowerShop.git
```

## Step 6: Push to GitHub

Push your code to GitHub:

```bash
git branch -M main
git push -u origin main
```

If prompted, enter your GitHub credentials or use a Personal Access Token.

## 🔑 Using Personal Access Token (Recommended)

GitHub no longer accepts passwords for Git operations. Use a Personal Access Token instead:

1. Go to GitHub Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Give it a name (e.g., "FlowerShop Project")
4. Select scopes: Check "repo" (full control of private repositories)
5. Click "Generate token"
6. **Copy the token immediately** (you won't see it again!)
7. When pushing, use the token as your password

## ✅ Verify Upload

1. Go to your GitHub repository in your browser
2. Refresh the page
3. You should see all your files uploaded
4. The README.md will be displayed on the repository homepage

## 🔄 Future Updates

After making changes to your code:

```bash
# Check status
git status

# Add changed files
git add .

# Commit changes
git commit -m "Description of changes"

# Push to GitHub
git push
```

## 🌿 Working with Branches

Create a new branch for features:

```bash
# Create and switch to new branch
git checkout -b feature/new-feature

# Make changes, then commit
git add .
git commit -m "Add new feature"

# Push branch to GitHub
git push -u origin feature/new-feature
```

## 🚨 Important Notes

- The `.gitignore` file is already configured to exclude:
  - Build artifacts (`bin/`, `obj/`)
  - Visual Studio files (`.vs/`)
  - User-specific files (`*.user`)
  - Database files (`*.mdf`, `*.ldf`)
  - Sensitive files

- **Never commit**:
  - Passwords or API keys
  - Database files
  - Personal access tokens
  - `appsettings.Production.json` with real connection strings

## 🔒 Protecting Sensitive Data

If you accidentally committed sensitive data:

```bash
# Remove file from Git but keep locally
git rm --cached <filename>

# Commit the removal
git commit -m "Remove sensitive file"

# Push changes
git push
```

For more serious cases, use [BFG Repo-Cleaner](https://rtyley.github.io/bfg-repo-cleaner/) or `git filter-branch`.

---

**Your FlowerShop project is now on GitHub! 🎉**
