import {useEditor, EditorContent} from '@tiptap/react';
import StarterKit from '@tiptap/starter-kit';

const RichTextEditor = ({ onChange }: { onChange: (html: string) => void }) => {
  const editor = useEditor({
    extensions: [StarterKit],
    content: "",
    onUpdate: ({ editor }) => {
      onChange(editor.getHTML());
    },
  });

  if (!editor) return null;

  return (
    <div className="space-y-2">
      {/* Toolbar */}
      <div className="flex flex-wrap gap-2 bg-gray-100 p-2 rounded-md">
        <button onClick={() => editor.chain().focus().toggleBold().run()} className={editor.isActive("bold") ? "btn-active" : "btn"}>
          Bold
        </button>
        <button onClick={() => editor.chain().focus().toggleItalic().run()} className={editor.isActive("italic") ? "btn-active" : "btn"}>
          Italic
        </button>
        <button onClick={() => editor.chain().focus().toggleCode().run()} className={editor.isActive("code") ? "btn-active" : "btn"}>
          Code
        </button>
        <button onClick={() => editor.chain().focus().toggleBulletList().run()} className={editor.isActive("bulletList") ? "btn-active" : "btn"}>
          • List
        </button>
        <button onClick={() => editor.chain().focus().toggleBlockquote().run()} className={editor.isActive("blockquote") ? "btn-active" : "btn"}>
          Quote
        </button>
      </div>

      {/* Editor */}
      <EditorContent editor={editor} className="border rounded-md p-4 bg-white min-h-[150px]" />
    </div>
  );
};

export default RichTextEditor;
