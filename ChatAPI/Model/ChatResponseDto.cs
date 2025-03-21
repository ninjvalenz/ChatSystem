using System;

namespace ChatAPI.Model
{
    public class ChatResponseDto
    {
        public Guid SessionId { get; set; }
        public bool IsSuccess { get; set; }
    }
}