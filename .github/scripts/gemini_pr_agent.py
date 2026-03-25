import os
import json
import glob
from google import genai
from google.genai import types

# 1. Setup API and Environment Variables
client = genai.Client(api_key=os.environ["GEMINI_API_KEY"])
issue_body = os.environ.get("ISSUE_BODY", "No issue description provided.")

# 2. Read the Rules and Docs
try:
    with open("gemni.md", "r") as f:
        system_rules = f.read()
except FileNotFoundError:
    system_rules = "You are a helpful .NET AI coding assistant."

docs_context = ""
for filepath in glob.glob("docs/**/*.md", recursive=True):
    with open(filepath, "r") as f:
        docs_context += f"\n--- {filepath} ---\n{f.read()}\n"

# 3. Build the Prompt asking for JSON output
prompt = f"""
You are an expert .NET developer. Fix the following issue:
{issue_body}

Project Documentation for context:
{docs_context}

Based on the issue and the docs, determine which files need to be created or modified.
Respond ONLY with a valid JSON array matching this exact schema:
[
  {{
    "file_path": "path/to/YourFile.cs",
    "content": "// The complete, updated C# code goes here"
  }}
]
"""

# 4. Call Gemini (Enforcing JSON output)
response = client.models.generate_content(
    model='gemini-2.0-flash',
    contents=prompt,
    config=types.GenerateContentConfig(
        system_instruction=system_rules,
        response_mime_type="application/json",
        temperature=0.1 # Keep it strictly logical
    )
)

# 5. Parse the JSON and write the new code to the files
try:
    files_to_update = json.loads(response.text)
    for file_data in files_to_update:
        file_path = file_data["file_path"]
        
        # Ensure directories exist if Gemini decides to create a new file
        os.makedirs(os.path.dirname(file_path), exist_ok=True)
        
        with open(file_path, "w") as f:
            f.write(file_data["content"])
            
        print(f"Successfully updated/created: {file_path}")
except Exception as e:
    print(f"Error parsing or writing files: {e}")
    print("Raw Output:", response.text)
