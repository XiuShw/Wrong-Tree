import os
import glob
import pyperclip

def get_cs_files_content(directory):
	result = []
	cs_files = glob.glob(os.path.join(directory, "*.cs"))
	for file_path in cs_files:
		filename = os.path.basename(file_path)
		with open(file_path, "r", encoding="utf-8") as f:
			content = f.read()
		result.append(f"filename: {filename}\ncontent:\n```\n{content}\n```")
	return "\n\n".join(result)

if __name__ == "__main__":
	current_dir = os.path.dirname(os.path.abspath(__file__))
	all_content = get_cs_files_content(current_dir)
	pyperclip.copy(all_content)
	print("All .cs file contents have been copied to the clipboard.")